using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using sh = Stas.GA.SpriteHelper;
using V2 = System.Numerics.Vector2;
using V3 = System.Numerics.Vector3;
namespace Stas.GA;
public partial class AreaInstance  {
    internal SW sw_ent_upd = new SW("Ent upd:");
    void UpdateEntities(StdMap ePtr, bool addToCache = true) {
        FrameClear();
        sw_ent_upd.Restart();
        var entities = ui.m.ReadStdMapAsList<EntityNodeKey, EntityNodeValue>(ePtr, EntityFilter.IgnoreVisualsAndDecorations);
        if (ui.b_contrl  && ui.sett.b_develop)
            entities = ui.m.ReadStdMapAsList<EntityNodeKey, EntityNodeValue>(ePtr, null);
        //entities = entities.OrderBy(s => s.Key.id).ToList();
        //sw_cash.Print("ReadStdMapAsList");
        var data = AwakeEntities;
        TryRemoveOldEntyty();
        TryRemoveExplodedEnt();
        TryGetEntToDebug();
        if (mi_debug != null)
            return;
        var e_added = 0;
#if DEBUG
        for (var i = 0; i < entities.Count; i++) {//for step-by-step debugging use here
            calc(i);
        }
#else
        Parallel.For(0, entities.Count, (i, state) => { //use for prodaction here
            calc(i);
        });
#endif
        void calc(int i) {
            var (key, value) = entities[i];
            if (key.id == ui.me.id) {
                frame_items.Add(AddMapItem(ui.me));
                return;
            }
            if (data.TryGetValue(key, out var e)) {
                e.Tick( value.EntityPtr);
            }
            else {
                e = new Entity(value.EntityPtr);
                e.Tick(value.EntityPtr);
                e_added += 1;
                if (!string.IsNullOrEmpty(e.Path)) {
                    data[key] = e;
                    if (addToCache) {
                        AddToCacheParallel(key, e.Path);
                    }
                }
                else {
                    e = null;
                }
            }

            if (e != null) {
                if (e.eType == eTypes.Useless && !ui.b_contrl)
                    return;
                if (frame_di != null)
                    return;
                Debug.Assert(e.eType != eTypes.Unidentified);
                var nmi = AddMapItem(e);//new map item
                if (nmi != null) {
                    frame_items.Add(nmi);
                    //SetDanger(e);
                }
            }
        }
        GetBlight();
        GetParty();
        danger = curr_danger;
        UpdMapTasks();//insded iTasks = frame_i_tasks;
        triggers = new ConcurrentBag<Cell>(frame_trigger);//рисуются отдельно
        var sorted = frame_items.OrderBy(e => e.ent.id).ToList();
        map_items = new ConcurrentBag<MapItem>(sorted);
        debug_info = ("ent=[" + data.Count + "/" + entities.Count + "/" + map_items.Count + "]");
        sw_ent_upd.MakeRes();
        void UpdMapTasks() {
            foreach (var ot in iTasks) {
                var cft = frame_i_tasks.FirstOrDefault(ft => ft.id == ot.id); //current frame task
                if (cft == null) {
                    var deleted = iTasks.Where(t => t.id != ot.id);
                    iTasks = new ConcurrentBag<iTask>(deleted);
                }
                else {
                    ot.to = cft.to;
                }
            }

            foreach (var ft in frame_i_tasks) {
                var old = iTasks.FirstOrDefault(t => t.id == ft.id);
                if (old == null)
                    iTasks.Add(ft);
            }
        }
        //remove all close ent poternsion not valid but still have isValid from server
        void TryRemoveExplodedEnt() {
            foreach (var kv in data) {
                if (!kv.Value.IsValid) {
                    //var dist = this.player.DistanceFrom(kv.Value);
                    var dist = kv.Value.gdist_to_me;
                    var bad_dist = kv.Value.CanExplode && dist < 150;
                    if (kv.Value.eType == eTypes.Friendly || bad_dist) {
                        /* This logic isn't perfect in case something happens to the entity before
                         we can cache the location of that entity. In that case we will just
                         delete that entity anyway. This activity is fine as long as it doesn't
                         crash the GameHelper. This logic is to detect if entity exploded due to
                         explodi-chest or just left the network bubble since entity leaving network
                         bubble is same as entity exploded.*/
                        data.TryRemove(kv.Key, out _);
                    }
                }
                kv.Value.IsValid = false;
            }
              
        }
        void TryRemoveOldEntyty() {
            if (ui.b_contrl)
                return;
            foreach (var kv in data) {
                var e = kv.Value;
                var so_faar = e.gdist_to_me > ui.sett.max_entyty_valid_gdistance;
                var is_dead = e.eType == eTypes.Monster && e.IsDead;
                if (!e.IsValid || so_faar || is_dead) { //dont delete misk etc for prevent remake it
                    var done = AwakeEntities.TryRemove(kv.Key, out _);
                    if (!done) {
                        ui.AddToLog("cant delete ent from cash", MessType.Error);
                    }
                }
            }
        }
    }
    void GetParty() {
        foreach (var b in ui.bots.Where(b => b.map_hash == ui.curr_map_hash)) {
            var bot_icon = MapIconsIndex.PartyMember;
            if (b.b_i_died)
                bot_icon = MapIconsIndex.I_died;
            var mi = new MapItem(b);
            mi.uv = sh.GetUV(bot_icon);
            frame_items.Add(mi);
        }
    }
    void TryGetEntToDebug() {
        if (ui.b_contrl || ui.b_alt) {//try pick debug entyty from last draw list
            if (frame_di == null) {
                foreach (var mi in static_items.Values) {
                    var _cd = mi.gpos.GetDistance(ui.MapPixelToGP);
                    if (_cd < 2) {
                        frame_di = mi;
                        break;
                    }
                }
            }
            if (frame_di == null) {
                foreach (var mi in map_items) {
                    var _cd = mi.ent.gpos.GetDistance(ui.MapPixelToGP);
                    if (_cd < 2) {
                        frame_di = mi;
                        break;
                    }
                }
            }
        }
        if (frame_di == null) {
            mi_debug = null;
        }
        else {
            mi_debug = frame_di;
        }
    }
    void FrameClear() {
       
        frame_i_tasks.Clear();
        frame_party.Clear();
        frame_party.Add(ui.me);
        frame_items.Clear();
        frame_blight.Clear();
        //exped_key_frame.Clear();
        //exped_beams_frame.Clear();
        frame_trigger.Clear();
        danger_enemy.Clear();
        enemy.Clear();
        curr_danger = 0;
        //curr_marked = null;
        frame_di = null;
        danger_cells.Clear();
        //frame_debug.Clear();
    }
}