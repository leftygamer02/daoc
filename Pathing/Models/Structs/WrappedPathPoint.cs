using System.Numerics;

namespace Pathing
{
    public struct WrappedPathPoint
    {
        public Vector3 Position;
        public dtPolyFlags Flags;

        public override string ToString()
        {
            return $"({Position}, {Flags})";
        }
    }
}
