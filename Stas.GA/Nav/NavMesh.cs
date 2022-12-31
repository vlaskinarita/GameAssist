using System;
using System.Collections.Generic;
using System.Text;
using V3 = System.Numerics.Vector3;
using V2 = System.Numerics.Vector2;
using System.Linq;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.IO;
using System.ComponentModel;
using System.Threading;

namespace Stas.GA {
    public partial class NavMesh{
        string tName => GetType().Name;
        public bool b_ready = false;
        //int GreedCellSizeI = 23; // map coords tiles are 23 x 23
        //float WorldCellSizeF = 250.0f; // world coords tiles are 250 x 250
        //float GreedToWorldScalar => WorldCellSizeF / GreedCellSizeI; // 10.869565f
        //float WorldToMapScalar => GreedCellSizeI / WorldCellSizeF; // 0.092f
        public int GetBitByGp(V2 gp) => data[(int)gp.X, (int)gp.Y];
        int[,] data => ui.curr_map.bit_data;
        public ConcurrentBag<GridCell> grid_cells = new ConcurrentBag<GridCell>();
        /// <summary>
        /// grid cell size
        /// </summary>
        public const int gcs = 23;
        public int lcid = 0; //last cell id
        Stopwatch sw = new Stopwatch();
        public float explored_percent; //percentage of area explored
        public ConcurrentBag<GridCell> grouts => _routs_gc;
        ConcurrentBag<GridCell> _routs_gc = new ConcurrentBag<GridCell>();
        public GridCell gc_test;
        public GridCell Get_gc_by_ent(Entity ent) => Get_gc_by_gp(ent.gpos);
        public GridCell Get_gc_by_gp(V2 gp) {
            return grid_cells.FirstOrDefault(g => g.min.X <= gp.X && g.min.Y <= gp.Y && g.max.X >= gp.X && g.max.Y >= gp.Y);
        }
        public bool IsVisited(Entity ent) => Get_gc_by_ent(ent).b_visited;
        public int visited_dist => 120; //distance for visited
        public V2 start_gpos;
        //this for visual denug NavGo task
        public static ConcurrentBag<(V2, string)> gpa = new();
        public static void Reset() {
            gpa.Clear();
        }
        Thread nav_thread;
        public NavMesh() {
            nav_thread = new Thread(()=> {
                while (ui.b_running) {
                    if (!b_ready ) {
                        Thread.Sleep(100);
                        continue;
                    }
                    var me_pos = ui.me.gpos_f;
                    if (me_pos.X <= 0 || me_pos.Y <= 0) {
                        ui.AddToLog(tName + ".worker_thread me.gpos==def", MessType.Error);
                        Thread.Sleep(200);
                        continue;
                    }
                    sw.Restart();

                    var mgc = Get_gc_by_gp(ui.me.gpos);
                    my_curr_cell = mgc;

                    if (!ui.sett.b_use_gh_map) {
                        CalcVisited();
                    }
                 
                    if (elaps.Count > 60)
                        elaps.RemoveAt(0);
                    elaps.Add(sw.Elapsed.TotalMilliseconds);
                    var ft = elaps.Sum() / elaps.Count; //frame time
                    var fps = Math.Round(1000f / ft, 1);
                    //ui.AddToLog("nav.Tick fps=[" + fps + "]");

                    #region w8ting
                    var t_elaps = (int)sw.Elapsed.TotalMilliseconds; //totale elaps
                    if (t_elaps < ui.w8) {
                        Thread.Sleep(ui.w8 - t_elaps);
                    }
                    if (t_elaps > ui.w8) {
                        Thread.Sleep(1);
                        ui.AddToLog("NavMesh: Big tick time=[" + t_elaps + "]", MessType.Warning);
                    }
                    #endregion
                }
            });
            nav_thread.IsBackground = true;
            nav_thread.Start();
        }

        void CalcVisited() {
            var oround = grouts.Where(v => !v.b_visited && v.center.GetDistance(ui.me.gpos) < visited_dist);
            foreach (var gc in oround) {
                if (!b_ready)
                    return; //This can happen if we change the map while this function is running.
                var sr = 0f; //total(summ) area of all routs
                var sv = 0f; //total(summ) area of visited routs
                foreach (var cl in gc.routs) {
                    sr += cl.area;
                    if (cl.center.GetDistance(ui.me.gpos) < visited_dist) {
                        cl.b_visited = true;
                        sv += cl.area;
                    }
                }
                if (sr > 0) {
                    gc.visited_persent = sv / sr;
                    if (gc.visited_persent == 1)
                        gc.b_visited = true;
                }
            }
            var explored = (float)_routs_gc.Where(gc => gc.visited_persent > 0.9f).ToList().Count;
            explored_percent = explored / _routs_gc.Count;
        }
        public Cell GetFirstRount(V2 gp) {
            return null;
          
        }
       
        public void GetNeighborForGridCell(GridCell curr, bool del_old = false) {
          
        }
        public void GetNeighbor(Cell cr, List<Cell> list_sells) {
           
        }
        public int gdist_to_me(V2 gp) {
            var res = GetRes(ui.me.gpos, gp);
            return (int)res.distance;
        }
       
        public NavRes debug_res;
        public bool b_can_hit(Entity target) {
            return b_can_hit(target.gpos);
        }
        public ConcurrentBag<Cell> test_cells = new ConcurrentBag<Cell>();
        List<double> elaps = new List<double>();
        public Cell my_curr_cell;
       
        public int GetBit(V3 pos) {
            var gpos = pos * ui.worldToGridScale;
            return data[(int)gpos.X, (int)gpos.Y];
        }
        public int GetBit(V2 gpos) {
            if(gpos.X >= data.GetLength(0) || gpos.X < 0 || gpos.Y >= data.GetLength(1) || gpos.Y <0) {
                var rx = gpos.X + "\\" + data.GetLength(0);
                var ry = gpos.Y + "\\" + data.GetLength(1);
                ui.AddToLog("GetBit err: [" + rx + "][" + ry + "]");
                return -1;
            }
            else
                return data[(int)gpos.X, (int)gpos.Y];
        }
      
    }
}
