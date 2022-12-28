using System;
using System.Collections.Generic;
using System.Text;
using V3 = System.Numerics.Vector3;
using V2 = System.Numerics.Vector2;
using System.IO;

namespace Stas.GA.Main
{
    public class LeaderFromNet
    {
        public V3 pos;
        public uint map_hash;
        public bool b_dead;
        public string leader_name;
        public bool b_valid => map_hash == ui.curr_map_hash;
        public V2 gpos
        {
            get
            {
                var v3g = pos * ui.worldToGridScale;
                return new V2(v3g.X, v3g.Y);
            }
        }

        public byte[] ToByte()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(pos.ToByte());
                    bw.Write(map_hash);
                    bw.Write(b_dead);
                    bw.Write(leader_name.To_UTF8_Byte());
                }
                return ms.ToArray();
            }
        }
        public void FillFromByteArray(byte[] ba, int offs = 0)
        { // офссет на опкод
            using (var ms = new MemoryStream(ba))
            {
                using (var br = new BinaryReader(ms))
                {
                    ms.Position = offs;
                    pos = br.ReadBytes(12).ToV3(0);
                    map_hash = br.ReadUInt32();
                    b_dead = br.ReadByte().ToBool();
                    leader_name = br.To_UTF8_String();
                }
            }
        }
    }
    public class Leader
    {
        public Entity ent = null;
        public LeaderFromNet lfn = new LeaderFromNet();
        public V3 pos
        {
            get
            {
                if (ent == null)
                {
                    if (lfn.b_valid)
                        return lfn.pos;
                    else
                        return V3.Zero;
                }
                else
                    return ent.pos;
            }
        }
        public V2 gpos
        {
            get
            {
                var v3c = pos * ui.worldToGridScale;
                return new V2(v3c.X, v3c.Y); ;
            }
        }
        public uint map_hash => ent == null ? lfn.map_hash : ui.curr_map_hash;
        public bool b_OK
        {
            get
            {
                if (ent == null)
                {
                    return lfn.map_hash == ui.curr_map_hash && !lfn.b_dead;
                }
                else
                {
                    return ent.IsValid && ent.IsAlive && ent.gpos != V2.Zero;
                }
            }
        }
        public float gdist_to_me
        {
            get
            {
                if (ent != null)
                    return ent.gdist_to_me;
                else
                {
                    if (lfn.map_hash == ui.curr_map_hash)
                        return ui.me.pos.GetDistance(lfn.pos) * ui.worldToGridScale;
                    return float.PositiveInfinity;
                }
            }
        }
    }
}
