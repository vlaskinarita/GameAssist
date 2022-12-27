using System.Collections.Concurrent;
using V2 = System.Numerics.Vector2;
using V3 = System.Numerics.Vector3;
using sh = Stas.GA.SpriteHelper;
using System.IO;

namespace Stas.GA;
public partial class AreaInstance {
    ConcurrentBag<Cell> frame_trigger = new();
    ConcurrentBag<MapItem> frame_items = new();
    public ConcurrentBag<iTask> iTasks = new ();
    ConcurrentBag<iTask> frame_i_tasks = new ();
    HashSet<string> quest_ent = new HashSet<string>();

    MapItem AddMapItem(Entity e) {
        if (e.pos == V3.Zero) {
            if (!bad_etypes.ContainsKey(e.eType)) {
                bad_etypes.TryAdd(e.eType, new ConcurrentDictionary<uint, Entity>());
            }
            bad_etypes[e.eType][e.id] = e;
            return null;
        }
        if (string.IsNullOrEmpty(e.Path) || e.eType == eTypes.Unidentified) { //!e.IsValid ||
            ui.AddToLog("AddMapItem error ent", MessType.Error);
            return null;
        }
        var gdist = e.gdist_to_me;
        var info = e.RenderName; //" d=" + gdist.ToRoundStr(0);
        if (string.IsNullOrEmpty(info)) {
            info = pa_info(e);
        }
        if (e.id == debug_id) {//debug_id
                               //  System.Diagnostics.Debugger.Break();
        }
        var mi = new MapItem(e, info);
        SetRarity(mi);
        switch (e.eType) { //order is importand here - > fallow GO
            case eTypes.MinimapIcon:
                return GetIcon(e, mi);
            case eTypes.Portal:
                return GetPortal(e);
            case eTypes.Door:
                return GetDoor(e);
            case eTypes.Quest: {
                    if (e.GetComp<MinimapIcon>(out _))
                        return asStaticMapItem(e, miType.Quest, MapIconsIndex.QuestItem);
                    else
                        return asStaticMapItem(e, miType.Quest, MapIconsIndex.QuestObject);
                }
            case eTypes.NPC:
                return GetNPC(e);
            case eTypes.OtherPlayer:
            case eTypes.SelfPlayer:
                return GetPlayer(e, mi);
            case eTypes.Blockage:
                DoWithTriggers(e);
                break;
            case eTypes.Chest://same chest types need update
            case eTypes.DelveChest:
            case eTypes.HeistChest:
            case eTypes.ImportantStrongboxChest:
            case eTypes.StrongboxChest:
            case eTypes.BreachChest:
            case eTypes.Barrel:
            case eTypes.LargeChest:
                return GetChest(e, mi);
            case eTypes.ExpeditionChest:
                mi.uv = sh.GetUV(MapIconsIndex.opened_chest_big);
                return mi;
            case eTypes.Shrine:
                if (e.GetComp<Shrine>(out var shrine)) {
                    if (shrine.IsUsed) //!target.isTargetable  => old method
                        return null;
                    mi.uv = sh.GetUV(MapIconsIndex.Shrine);
                    mi.size = ui.sett.icon_size + 6;
                    Set_GridCell_as_rout(e.gpos);
                    return mi;
                }
                else {
                    ui.AddToLog("AddEnt err for for Shrine", MessType.Error);
                    return null;
                }
            case eTypes.Stage0FIT:
            case eTypes.Stage1FIT:
            case eTypes.Friendly:
            case eTypes.Pet:
            case eTypes.Monster:
                return GetMonster(e, mi);
            case eTypes.Stage0RewardFIT:
            case eTypes.Stage1RewardFIT:
            case eTypes.Stage0EChestFIT:
            case eTypes.Stage1EChestFIT:
                mi.uv = sh.GetUV(MapIconsIndex.opened_chest_midle); //"Legion Reward Monster Chest";
                if (e.eType == eTypes.Stage0EChestFIT || e.eType == eTypes.Stage1EChestFIT) {
                    mi.uv = sh.GetUV(MapIconsIndex.BigChest);// "Legion Epic Chest";
                }
                return mi;
            case eTypes.DeliriumBomb:
                mi.uv = sh.GetUV(MapIconsIndex.RedFlag);
                return mi;
            case eTypes.DeliriumSpawner:
                mi.uv = sh.GetUV(MapIconsIndex.GreenFlag);
                return mi;
            case eTypes.WorldItem: {
                    var ce = e.tName + " type=" + e.eType;
                    mi.uv = sh.GetUV(MapIconsIndex.Amulet);
                    return mi;
                }
            case eTypes.LimitedLife: {
                    if (e.Path.EndsWith("GroundBlade"))
                        return null;
                    break;
                }
            case eTypes.Effects: {
                    mi.uv = sh.GetUV(MapIconsIndex.Effect);
                    return mi;
                }
            default: {
                    if (ui.b_contrl) {
                        mi.uv = sh.GetUV(MapIconsIndex.BlightPathInactive);
                        return mi;
                    }
                    var ce = e.tName + " type=" + e.eType;
                    return null;
                }
        }
        return null;
    }
    MapItem GetNPC(Entity e) {
        e.GetComp<NPC>(out var npc);
        e.GetComp<MinimapIcon>(out var icon);
        if (npc == null || icon == null) {
            ui.AddToLog("AddmapItem err: GetNPC", MessType.Error);
            return null;
        }
        var mii = MapIconsIndex.NPC;
        if (e.Path.StartsWith("Metadata/NPC/League/Cadiro"))
            mii = MapIconsIndex.QuestObject;
        else if (e.Path.StartsWith("Metadata/Monsters/LeagueBetrayal/MasterNinjaCop"))
            mii = MapIconsIndex.BetrayalSymbolDjinn;
        if (npc.HasIconOverhead && (icon != null && !icon.IsHide)) {
            mii = MapIconsIndex.QuestObject;
        }
        return asStaticMapItem(e, miType.NPC, mii);
    }
    ConcurrentBag<Entity> frame_party = new();
    string pa_info(Entity e) {
        if (!id_ifos.ContainsKey(e.id)) {
            lock (id_ifos) {
                var pa = e.Path.Split('/');
                id_ifos[e.id] = pa[pa.Length - 1];
            }
        }
        return id_ifos[e.id];
    }
}
