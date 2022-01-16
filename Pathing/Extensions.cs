using System.Numerics;

namespace Pathing
{
    static class Extensions
    {
        public static float[] ToRecastFloats(this Vector3 value)
        {
            return new[] {
                (float) (value.X * PathingServiceImpl.CONVERSION_FACTOR),
                (float) (value.Z * PathingServiceImpl.CONVERSION_FACTOR),
                (float) (value.Y * PathingServiceImpl.CONVERSION_FACTOR)
            };
        }
    }
}
