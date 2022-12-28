using System.Diagnostics;
using System.Drawing;
namespace Stas.GA;
public partial class AreaInstance {
    Stopwatch sw = new Stopwatch();
    public IntPtr map_ptr;
    public float progress = 0f;
    public int rows;
    public int cols;
    public int[,] bit_data { get; private set; }
    public bool b_added_col { get; private set; }
    int make_ticks = 0; //for debug time creating
    /// <summary>
    /// after map creating only
    /// </summary>
    public bool b_ready { get; private set; }
    /// <summary>
    /// must be run in separate thread only
    /// </summary>
    public void UpdateMap(object state) {
        ClearOldData();
        UpdMapImage();

        ui.nav.MakeGridSells();//here first coz ve need cell to dtae quest area
        ui.nav.LoadVisited();
        ui.curr_map.GetTileTgtName();
        ui.LoadQuest();
        ui.area_change_counter.Tick(ui.area_change_counter.Address, "UpdateMap");
        if (!ui.b_town && !ui.b_home)
            ui.curr_loaded_files.Load(tName);
        //ui.looter.LoadOldLoot();
    }
    void UpdMapImage() {
        int curr_w8 = 0;
        int w8 = 3;
        AreaInstanceOffsets data = default;
        while (data.TerrainMetadata.BytesPerRow <= 0
            || data.TerrainMetadata.GridWalkableData.Size <= 200 * 200
            || data.TerrainMetadata.GridWalkableData.Size > 4000 * 4000) {
            Thread.Sleep(w8);
            b_ready = false;
            ui.AddToLog(tName + ".UpdMapImage w8=[" + (curr_w8 += w8) + "]", MessType.Warning);
            ui.AddToLog(tName + ".UpdMapImage w8 right data...", MessType.Warning);
            if (Address != default)
                data = ui.m.Read<AreaInstanceOffsets>(Address);
        }
        terr_meta_data = data.TerrainMetadata;
        walkable_data = ui.m.ReadStdVector<byte>(terr_meta_data.GridWalkableData);
        height_data = GetTerrainHeight();
        List<byte> valid = new List<byte>();
        for (byte f = 0; f <= 5; f++) {
            for (byte s = 0; s <= 5; s++) {
                byte res = (byte)((f << 4) | s);
                //var res_h = res.ToString("X");
                //var res_b = Convert.ToString(res, 2);
                //Console.WriteLine(res_h + "=" + res_b);
                valid.Add(res);
            }
        }
        sw.Restart();
        var td = terr_meta_data;
        cols = (int)td.TotalTiles.X * 23;
        rows = (int)td.TotalTiles.Y * 23;
        var bytesPerRow = td.BytesPerRow;
        Debug.Assert(bytesPerRow > 0);
        if ((cols & 1) > 0) {
            cols++;
            b_added_col = true;
        }
        else
            b_added_col = false;
        bit_data = new int[cols, rows];
        var bmp = new Bitmap(bytesPerRow * 2, walkable_data.Length / bytesPerRow);
#if DEBUG
        for (int y = 0; y < height_data.Length; y++) {
            Run(y);
        }
#else
        Parallel.For(0, height_data.Length, y => {
            Run(y);
        });
#endif
        void Run(int y) {
            for (var x = 1; x < height_data[y].Length - 1; x++) {
                var index = (y * bytesPerRow) + (x / 2); // (x / 2) => since there are 2 data points in 1 byte.
                var shift = x % 2 == 0 ? 0 : 4;
                var both = walkable_data[index];
                Debug.Assert(valid.Contains(both));
                var bit = both >> shift & 0xF;
                bit_data[x, y] = bit;
                if (bit == 0)
                    continue;
                //TODO temporary - mb need filling array[color, color] or mb array[byte[4], byte[4]]
                //https://swharden.com/csdv/system.drawing/array-to-image/
                lock (bmp) { //we need it coz using Parallel
                    bmp.SetPixel(x, y, GetColor(bit));
                }
             }
        }
        map_ptr = ui.GetPtrFromImageData(bmp);
        bmp.Dispose();
        b_ready = true;
        ui.AddToLog("Map create time=[" + sw.ElapsedTostring() + "]", MessType.Warning);
    }
    void ClearOldData() {
        ui.sett.map_scale = ui.sett.map_scale_def;
        //quest_ent.Clear(); //mb don't
        b_ready = false;
        ui.elements.Clear();
        ui.w8ting_click_until.Clear();
        environmentPtr = default;
        environments.Clear();
        MonsterLevel = 0;
        AreaHash = 0;
        server_data.Tick(IntPtr.Zero);
        //player.Tick(IntPtr.Zero, tName+ ".ClearOldData");
        terr_meta_data = default;
        height_data = Array.Empty<float[]>();
        walkable_data = Array.Empty<byte>();
        TgtTilesLocations.Clear();
        blight_pamp = null;
        blight_beams.Clear();
        bad_etypes.Clear();
        id_ifos.Clear();
        bad_map_items.Clear();
        static_items.Clear();
        make_ticks = 0;
        exped_keys.Clear();
        exped_beams.Clear();
        ui.nav.b_ready = false;//for not draw old visited
        ui.nav.debug_res = null;//same oldes debug must be deleted
        ui.test?.spa?.Clear(); //debug data need only actuale
    }

    internal static Color GetColor(int i) {
        Color res;
        switch (i) {
            case 0:
                res = Color.FromArgb(0, 0, 0, 0);
                break;
            case 1:
                res = Color.FromArgb(20, 255, 255, 255);
                break;
            case 2:
                res = Color.FromArgb(50, 255, 255, 255);
                break;
            case 3:
                res = Color.FromArgb(90, 255, 255, 255);
                break;
            case 4:
                res = Color.FromArgb(25, 255, 255, 255);
                break;
            case 5:
                res = Color.FromArgb(15, 255, 255, 255);
                break;
            default:
                throw new Exception(i.ToString());
        }
        return res;
    }
}