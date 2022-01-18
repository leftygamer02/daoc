using Google.Protobuf;
using Grpc.Core;
using Nodes.Pathing;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Pathing
{
    public class PathingServiceImpl : PathingService.PathingServiceBase
    {
        public const float CONVERSION_FACTOR = 1.0f / 32f;
        private const float INV_FACTOR = (1f / CONVERSION_FACTOR);


        [DllImport("ReUth", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern bool LoadNavMesh(string file, ref IntPtr meshPtr, ref IntPtr queryPtr);

        [DllImport("ReUth", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool FreeNavMesh(IntPtr meshPtr, IntPtr queryPtr);

        [DllImport("ReUth", CallingConvention = CallingConvention.Cdecl)]
        private static extern dtStatus PathStraight(IntPtr queryPtr,
            float[] start,
            float[] end,
            float[] polyPickExt,
            dtPolyFlags[] queryFilter,
            dtStraightPathOptions pathOptions,
            ref int pointCount,
            float[] pointBuffer,
            dtPolyFlags[] pointFlags);

        [DllImport("ReUth", CallingConvention = CallingConvention.Cdecl)]
        private static extern dtStatus FindRandomPointAroundCircle(IntPtr queryPtr,
            float[] center,
            float radius,
            float[] polyPickExt,
            dtPolyFlags[] queryFilter,
            float[] outputVector);

        [DllImport("ReUth", CallingConvention = CallingConvention.Cdecl)]
        private static extern dtStatus FindClosestPoint(IntPtr queryPtr, float[] center, float[] polyPickExt, dtPolyFlags[] queryFilter, float[] outputVector);

        [Flags]
        private enum dtStatus : uint
        {
            // High level status.
            DT_SUCCESS = 1u << 30, // Operation succeed.

            // Detail information for status.
            DT_PARTIAL_RESULT = 1 << 6, // Query did not reach the end location, returning best guess. 
        }

        public enum dtStraightPathOptions : uint
        {
            DT_STRAIGHTPATH_NO_CROSSINGS = 0x00, // Do not add extra vertices on polygon edge crossings.
            DT_STRAIGHTPATH_AREA_CROSSINGS = 0x01, // Add a vertex at every polygon edge crossing where area changes.
            DT_STRAIGHTPATH_ALL_CROSSINGS = 0x02, // Add a vertex at every polygon edge crossing.
        }

        private const int MAX_POLY = 256; // max vector3 when looking up a path (for straight paths too)


        public const int ZoneCountMax = 500;

        private static (IntPtr navMeshPtr, IntPtr queryPtr, object lockObject)[] Pointers = new (IntPtr navMeshPtr, IntPtr queryPtr, object lockObject)[ZoneCountMax];
        private static bool[] PathingEnabled = new bool[ZoneCountMax];

        public static Vector3 ToVector3(Vec3 vec)
        {
            return new Vector3((float)vec.X, (float)vec.Y, (float)vec.Z);
        }

        public static Vec3 ToVec3(Vector3 vec)
        {
            return new Vec3 { X = vec.X, Y = vec.Y, Z = vec.Z };
        }

        public override Task<ListLoadedNavmeshesResponse> ListLoadedNavmeshes(ListLoadedNavmeshesRequest request, ServerCallContext context)
        {
            return Task.FromResult(new ListLoadedNavmeshesResponse { Navmeshes = { ZoneIdsWithPathing } });
        }

        public override Task<PathingResponse> GetPathStraight(PathingRequest request, ServerCallContext context)
        {
            if (!PathingEnabled[request.Navmesh])
                return Task.FromResult(new PathingResponse { ResultCode = PathingResult.NavmeshUnavailable, SequenceID = request.SequenceID });

            var start = ToVector3(request.StartingPoint);
            var dest = ToVector3(request.DestinationPoint);

            var foo = GetPathStraight(request.Navmesh, start, dest);

            if (foo.Points != null)
            {
                var numNodes = foo.Points.Length;
                var oneStructSize = Marshal.SizeOf<PathPointStruct>();
                byte[] buf = new byte[oneStructSize * numNodes];
                var span = MemoryMarshal.Cast<byte, PathPointStruct>(buf);

                for (int i = 0; i < numNodes; i++)
                {

                    var pp = foo.Points[i];
                    var ppr = new PathPointStruct
                    {
                        Flags = pp.Flags,
                        X = pp.Position.X,
                        Y = pp.Position.Y,
                        Z = pp.Position.Z,
                    };

                    span[i] = ppr;

                }

                return Task.FromResult(new PathingResponse
                {
                    ResultCode = (PathingResult)foo.Error,
                    SequenceID = request.SequenceID,
                    PathNodes = (uint)foo.Points.Length,
                    SerializedNodes = ByteString.CopyFrom(buf, 0, buf.Length)
                });
            }

            return Task.FromResult(new PathingResponse
            {
                ResultCode = (PathingResult)foo.Error,
                SequenceID = request.SequenceID
            });
        }

        public void Initialize()
        {
            var pathingBasePath = Path.Combine(new FileInfo(Assembly.GetEntryAssembly().Location).DirectoryName, "opt\\navmeshes");

            for (int i = 0; i < ZoneCountMax; i++)
            {
                var id = i;
                var file = Path.Combine(pathingBasePath, $"zone{id:D3}.nav");
                if (!File.Exists(file))
                {
                    //log.Error(string.Format("Loading NavMesh failed for zone {0}! (File not found: {1})", id, file));
                    continue;
                }

                var meshPtr = IntPtr.Zero;
                var queryPtr = IntPtr.Zero;

                if (!LoadNavMesh(file, ref meshPtr, ref queryPtr))
                {
                    return;
                }

                if (meshPtr == IntPtr.Zero || queryPtr == IntPtr.Zero)
                {
                    return;
                }

                Pointers[i] = (meshPtr, queryPtr, new object());
                PathingEnabled[i] = true;
            }

            var proxiedZones = new List<(int, int)>
            {
                // mid (region 73) is the original, alb (region 30) and hib (region 130) proxy to it in the game files

                // alb 
                (30, 73), // hespe 
                (31, 74), // meso
                (32, 75), // boeral
                (33, 76), // notos
                (34, 77), // anatole
                (38, 81), // stygia
                (39, 82), // atum
                (41, 84), // typhons reach
                (42, 85), // ashen isle
                (43, 86), // green glades
                (44, 87), // abor glen

                // hib 
                (130, 73), // hespe 
                (131, 74), // meso
                (132, 75), // boeral
                (133, 76), // notos
                (134, 77), // anatole
                (138, 81), // stygia
                (139, 82), // atum
                (141, 84), // typhons reach
                (142, 85), // ashen isle
                (143, 86), // green glades
                (144, 87), // abor glen
            };

            foreach (var tup in proxiedZones)
            {
                PathingEnabled[tup.Item1] = PathingEnabled[tup.Item2];
                Pointers[tup.Item1] = Pointers[tup.Item2];
            }

            ZoneIdsWithPathing = PathingEnabled.Select((a, b) => (a, b)).Where((t) => t.Item1).Select(t => (uint)t.Item2)
                .ToArray();
        }

        private uint[] ZoneIdsWithPathing;

        private WrappedPathingResult GetPathStraight(uint zoneId, Vector3 start, Vector3 end)
        {
            WrappedPathingResult pth;

            {
                var startFloats = (start + Vector3.UnitZ * 8).ToRecastFloats();
                var endFloats = (end + Vector3.UnitZ * 8).ToRecastFloats();

                var numNodes = 0;
                var buffer = new float[MAX_POLY * 3];
                var flags = new dtPolyFlags[MAX_POLY];
                dtPolyFlags includeFilter = dtPolyFlags.ALL ^ dtPolyFlags.DISABLED;
                dtPolyFlags excludeFilter = 0;
                float polyExtX = 64.0f;
                float polyExtY = 64.0f;
                float polyExtZ = 256.0f;
                dtStraightPathOptions options = dtStraightPathOptions.DT_STRAIGHTPATH_ALL_CROSSINGS;
                var filter = new[]
                {
                    includeFilter,
                    excludeFilter
                };

                dtStatus status;

                var ptrs = Pointers[zoneId];

                lock (ptrs.lockObject)
                {
                    status = PathStraight(
                        ptrs.queryPtr,
                        startFloats,
                        endFloats,
                        new Vector3(polyExtX, polyExtY, polyExtZ).ToRecastFloats(),
                        filter,
                        options,
                        ref numNodes,
                        buffer,
                        flags);
                }

                if ((status & dtStatus.DT_SUCCESS) == 0)
                {
                    return new WrappedPathingResult
                    {
                        Error = PathingError.NoPathFound,
                        Points = null,
                    };
                }

                var points = new WrappedPathPoint[numNodes];
                var positions = Vector3ArrayFromRecastFloats(buffer, numNodes);

                for (var i = 0; i < numNodes; i++)
                {
                    points[i].Position = positions[i];
                    points[i].Flags = flags[i];
                }

                ImprovePath(ptrs.queryPtr, ptrs.lockObject, points);

                if ((status & dtStatus.DT_PARTIAL_RESULT) == 0 && Vector3.Distance(end, positions[numNodes - 1]) <= 75)
                {
                    pth = new WrappedPathingResult
                    {
                        Error = PathingError.PathFound,
                        Points = points,
                    };
                }
                else
                {
                    pth = new WrappedPathingResult
                    {
                        Error = PathingError.PartialPathFound,
                        Points = points,
                    };
                }

                //zone.AddPath(start, end, pth);
            }


            return pth;
        }

        #region Path Improvement

        /// <summary>
        /// Curve dist improvement factor
        /// </summary>
        private const float CURVE_DIST_FACTOR = 1.1f;

        /// <summary>
        /// Maximum distance a curve point can be moved
        /// </summary>
        private const float CURVE_MAX_DIST = 80f;

        private void ImprovePath(IntPtr queryPointer, object lockObject, WrappedPathPoint[] recastPath)
        {
            if (recastPath.Length <= 2)
                return;

            // Assuming that we have three points A --> M ---> C, and we encounter any kind of a collision, then point
            // M will be sharply "bent" around the colliding object (since recast picks points very close to the object.
            // This means that the evil object is always in the inner side of the curve A --> M --> C, so by moving the point
            // M further "outwards" from the curve we can make our paths smoother.
            Vector3 oldM = Vector3.Zero;

            for (int i = 0; i < recastPath.Length - 2; i++)
            {
                var a3 = (oldM == Vector3.Zero ? recastPath[i + 0].Position : oldM);
                // http://mathworld.wolfram.com/Point-LineDistance3-Dimensional.html
                var pointA = new Vector3(a3.X, a3.Y, 0);
                var pointM = new Vector3(recastPath[i + 1].Position.X, recastPath[i + 1].Position.Y, 0);
                var pointB = new Vector3(recastPath[i + 2].Position.X, recastPath[i + 2].Position.Y, 0);

                // Calculate the line between A --> C
                var lineOrigin = pointA;
                var lineDir = (pointB - pointA);
                oldM = pointM;

                // Figure out where pointM is closest to that line
                var distAB = Vector3.DistanceSquared(pointA, pointB);
                if (distAB <= 0.1 || pointA == pointM)
                {
                    continue;
                }

                var t = -Vector3.Dot(pointA - pointM, lineDir) / distAB;
                var closestPoint = lineOrigin + lineDir * t;

                // Add a bit of this distance to point M to move it outwards
                var offsetVector = (pointM - closestPoint) * CURVE_DIST_FACTOR;
                var offsetMagnitude = offsetVector.Length();
                if (offsetMagnitude > CURVE_MAX_DIST)
                {
                    offsetVector = offsetVector * CURVE_MAX_DIST / offsetMagnitude;
                }

                if (offsetVector.Length() > 4096)
                {
                    //log.Error($"PathCalculator.ImprovePath returned very large offset ({offsetVector}) for A={pointA}, M={pointM}, B={pointB} for this={this}");
                }

                var newPos = recastPath[i + 1].Position + offsetVector;
                newPos = GetClosestPoint(queryPointer, lockObject, newPos, xRange: 64f, yRange: 64f, zRange: 128f) ?? newPos;
                recastPath[i + 1].Position = newPos;
            }
        }

        #endregion

        public override Task<GetClosestPointResponse> GetClosestPoint(GetClosestPointRequest request, ServerCallContext context)
        {
            if (!PathingEnabled[request.Navmesh])
                return Task.FromResult(new GetClosestPointResponse { Point = request.Position });

            var start = ToVector3(request.Position);

            var tup = Pointers[request.Navmesh];
            var vec = GetClosestPoint(tup.queryPtr, tup.lockObject, start);

            if (vec.HasValue)
            {
                return Task.FromResult(new GetClosestPointResponse { Point = ToVec3(vec.Value) });
            }

            return Task.FromResult(new GetClosestPointResponse { Point = request.Position });
        }

        /// <summary>
        /// Returns the closest point on the navmesh (UNTESTED! EXPERIMENTAL! WILL GO SUPERNOVA ON USE! MAYBE!?)
        /// </summary>
        private Vector3? GetClosestPoint(IntPtr queryPointer, object lockObject, Vector3 position, float xRange = 256f,
            float yRange = 256f, float zRange = 256f)
        {
            var center = (position + Vector3.UnitZ * 8).ToRecastFloats();
            var outVec = new float[3];

            var defaultInclude = (dtPolyFlags.ALL ^ dtPolyFlags.DISABLED);
            var defaultExclude = (dtPolyFlags)0;
            var filter = new dtPolyFlags[]
            {
                defaultInclude,
                defaultExclude
            };

            var polyPickEx = new Vector3(xRange, yRange, zRange).ToRecastFloats();

            lock (lockObject)
            {
                var status = FindClosestPoint(queryPointer, center, polyPickEx, filter, outVec);

                if ((status & dtStatus.DT_SUCCESS) == 0)
                {
                    return (Vector3?)null;
                }
            }

            return new Vector3(outVec[0] * INV_FACTOR, outVec[2] * INV_FACTOR, outVec[1] * INV_FACTOR);
        }

        private Vector3[] Vector3ArrayFromRecastFloats(float[] buffer, int numNodes)
        {
            var result = new Vector3[numNodes];
            for (var i = 0; i < numNodes; i++)
                result[i] = new Vector3(buffer[i * 3 + 0] * INV_FACTOR, buffer[i * 3 + 2] * INV_FACTOR, buffer[i * 3 + 1] * INV_FACTOR);
            return result;
        }
    }
}
