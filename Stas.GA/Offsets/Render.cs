using System.Runtime.InteropServices;
using V2 = System.Numerics.Vector2;
using V3 = System.Numerics.Vector3;
namespace Stas.GA;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
public struct RenderOffsets {
    [FieldOffset(0x0000)] public ComponentHeader Header;
    [FieldOffset(0x80 + 0x18)] public V3 CurrentWorldPosition;
    [FieldOffset(0xA0)] public NativeStringU name;
    [FieldOffset(0x80 + 0x18 + 12)] public StdTuple3D<float> CharactorModelBounds;
    [FieldOffset(0xF0 + 0x0C)] public float TerrainHeight;
}

