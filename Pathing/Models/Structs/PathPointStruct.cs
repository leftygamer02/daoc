using System.Runtime.InteropServices;

namespace Pathing
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PathPointStruct
    {
        public dtPolyFlags Flags;
        public float X;
        public float Y;
        public float Z;
    }
}
