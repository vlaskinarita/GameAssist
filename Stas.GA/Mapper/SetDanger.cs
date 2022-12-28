#region using
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Color = System.Drawing.Color;
using V2 = System.Numerics.Vector2;
using V3 = System.Numerics.Vector3;
using sh = Stas.GA.SpriteHelper;
using System.Collections.Concurrent;
using System;

#endregion

namespace Stas.GA {
    public partial class AreaInstance {
        public float danger { get; private set; }
        List<double> d_elaps = new List<double>();
        public ConcurrentBag<Entity> danger_enemy = new ConcurrentBag<Entity>();
        public ConcurrentBag<Entity> enemy = new ConcurrentBag<Entity>();
        float curr_danger;
        void SetDanger(Entity e) {
            if (e == null || !e.IsValid || e.danger == 0 || e.gdist_to_me > 120) //|| !e.IsTargetable
                return;
          
            e.danger_k = 1; //reset it
            e.GetComp<Actor>(out var actor);

            if (e.buffs!=null && e.buffs.StatusEffects.Any(x => x.Key.EndsWith("_mark"))) {
                ui.curr_map.marked =e;
            }
            if (e.Stats != null ) {
                if (e.Stats.TryGetValue(GameStat.CannotBeDamaged, out var _cbd) && _cbd == 1)
                    return;
                if (e.Stats.TryGetValue(GameStat.CannotDie, out var _cd) && _cd == 1)
                    return;
            }
            enemy.Add(e);
            SetCell(e.gpos);
            e.GetComp<Pathfinding>(out var tpf); //test path finding
            //if (tpf != null) { 
            
            //}
            //for serching offsets run here
            //if (actor != null) {
            //    DebugActer(e);
            //    return;
            //}
            //if(!ui.nav.b_can_hit(i.ent))
            //   continue;
            iTask new_it = null;
            if (e.Path.Contains("Necromancer") && ui.nav.b_can_hit(e)) {
                new_it = new EnemyTask(e.id, e, default,  "Necro");
                danger_enemy.Add(e);
                curr_danger += e.danger_rt;
                return;
            }
           
            if (actor != null) {
                var aw = actor.CurrentAction;
                if (actor.Action == ActionFlags.unknow_512) {
                    ui.AddToLog("SetDanger action=[" + actor.Action.ToString() + "]", MessType.Warning);
                } else if (actor.Action == ActionFlags.unknow_1024) {
                    ui.AddToLog("SetDanger action=[" + actor.Action.ToString() + "]", MessType.Warning);
                }
                if (aw != null) {
                    var trg = aw.Target_ent;
                    if (actor.Action.HasFlag(ActionFlags.Moving)) {
                        var tgp = aw.tgp;// pf.TargetMovePos.ToVector2();
                        if (trg != null && trg.IsValid ) {
                            foreach (var p in frame_party) {
                                var gdist = tgp.GetDistance(p.gpos_f);
                                if (gdist < 6) {
                                    e.danger_k = 1.5f;
                                    new_it = new EnemyTask(e.id, e, tgp, "Move");
                                    SetCell(tgp);
                                    break;
                                }
                            }
                        }
                    }
                    if (actor.Action.HasFlag(ActionFlags.UsingAbility)) {
                        foreach (var p in frame_party) {
                            if (trg != null && trg.Address == p.Address) {
                                e.danger_k = 2;
                                danger_enemy.Add(e);
                                e.target = trg;
                                new_it ??= new EnemyTask(e.id, e, trg.gpos_f, "Hit");
                                var _sn = "Skill"; //skill name; cant be reading right now but we will try mb next time
                                var skill = actor.CurrentAction.skill;
                                if (actor.CurrentAction.Address!=default
                                    && skill.Address!=default) {//try get skill name
                                    if (!string.IsNullOrEmpty(skill.Name)) {
                                    }
                                    else if (!string.IsNullOrEmpty(skill.InternalName)) { 
                                    }
                                }
                                var dist = aw.tgp.GetDistance(p.gpos_f);
                                if (dist < 20) {
                                    new_it.info = _sn; //"Hit" => "Skill"
                                    e.danger_k = 3; //2=>3
                                    SetCell(aw.tgp);
                                }
                                break;
                            }
                        }
                    }
                }
            }
            if (new_it == null) {   //try get from path
                e.GetComp<Pathfinding>(out var pf);
                if (pf != null) {
                    var tgp = pf.TargetMovePos.ToVector2();
                    if (tgp == V2.Zero) {
                        //ui.AddToLog("pf.TargetMovePos == Zero");
                        return;
                    }
                    foreach (var p in frame_party) {
                        var gdist = tgp.GetDistance(p.gpos_f);
                        if (gdist < 6) {
                            e.danger_k = 1.5f;
                            new_it = new EnemyTask(e.id, e, tgp, "PF_move");
                            SetCell(tgp);
                            break;
                        }
                    }
                }
            }
            if (new_it != null) {
                frame_i_tasks.Add(new_it);
                danger_enemy.Add(e);
                curr_danger += e.danger_rt;
            }
        }
        public ConcurrentDictionary<int, Cell> danger_cells = new ConcurrentDictionary<int, Cell>();
        void SetCell(V2 gpos) {
            if (!ui.nav.b_ready || gpos.X <=0 || gpos.Y <= 0)
                return;
            var gc = ui.nav.Get_gc_by_gp(gpos);
            var cell = gc?.Get_rout_by_gp(gpos);
            if (cell == null) {
                return;
            }
            danger_cells[cell.id]= cell;
        }
     
    }
}