using System.Linq;
using V3 = System.Numerics.Vector3;
using V2 = System.Numerics.Vector2;
namespace Stas.GA {
    public partial class NavMesh {
        /// <summary>
        /// MUst be call from new thread
        /// </summary>
        public void MakeGridSells() {
            MakeRouts();
            if(!ui.sett.b_use_gh_map)
                LoadVisited();
            start_gpos = ui.my_last_gpos = ui.me.gpos;
        }

        void MakeRouts() {
            sw.Restart();
            _routs_gc.Clear();
            int id = -1;
            var rows = data.GetLength(1);
            var cols = data.GetLength(0);
            if (ui.curr_map.b_added_col)
                cols = cols - 1;
            grid_cells.Clear();
            for (int y = 0; y < rows; y += gcs) { //966
                for (int x = 0; x < cols; x += gcs) { //1794
                    var ngc = new GridCell(id += 1, new V2(x, y), new V2(x + gcs, y + gcs));
                    grid_cells.Add(ngc);
                    if (ngc.routs.Count > 0)
                        //if ((ngc.routs.Sum(r => r.area) / (gcs* gcs)) > (1f / gcs)) //Sum(r=>r.width*r.height) > 10
                        _routs_gc.Add(ngc);
                }
            }
            b_ready = true;
            ui.AddToLog("Make Grid Sells time=[" + sw.ElapsedTostring() + "]", MessType.Warning);
        }
    }
}
