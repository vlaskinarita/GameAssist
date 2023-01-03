namespace Stas.GA {
    using System;
    using System.Runtime.InteropServices;


    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct ServerDataStructure {
        public const int SKIP = 0x8000; // for reducing struct size.
        [FieldOffset(0x1818)] public StdVector PlayerInventories; //0x1818  3.20
        [FieldOffset(0x9288 + 0x160 - SKIP)] public readonly SkillBarIdsStruct SkillBarIds;
        [FieldOffset(0x1240)] public readonly StdVector NearestPlayers; // 3.20
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct InventoryArrayStruct {
        [FieldOffset(0x00)] public int InventoryId;
        [FieldOffset(0x04)] public int PAD_0;
        [FieldOffset(0x08)] public IntPtr InventoryPtr0; // InventoryStruct
        [FieldOffset(0x10)] public IntPtr InventoryPtr1; // this points to 0x10 bytes before InventoryPtr0
        [FieldOffset(0x18)] public long PAD_1;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SkillBarIdsStruct {
        public readonly ushort SkillBar1;
        public readonly ushort SkillBar2;
        public readonly ushort SkillBar3;
        public readonly ushort SkillBar4;
        public readonly ushort SkillBar5;
        public readonly ushort SkillBar6;
        public readonly ushort SkillBar7;
        public readonly ushort SkillBar8;
        public readonly ushort SkillBar9;
        public readonly ushort SkillBar10;
        public readonly ushort SkillBar11;
        public readonly ushort SkillBar12;
        public readonly ushort SkillBar13;
    }
}
