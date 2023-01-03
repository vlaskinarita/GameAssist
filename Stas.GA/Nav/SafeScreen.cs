using Newtonsoft.Json;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

using V2 = System.Numerics.Vector2;
using V3 = System.Numerics.Vector3;

namespace Stas.GA;
public class SafeScreen : iSett {

    [JsonProperty]
    public int Width { get; protected private set; }
    [JsonProperty]
    public int Height { get; protected private set; }

    public Dictionary<string, Cell> Blocks = new();
    public Dictionary<string, Cell> Rounts = new();
    public Dictionary<string, Cell> Centr = new();
    List<(string, V2, V2)> CentrPoints = new();
    List<(string, V2, V2)> RoutPounts = new();
    [JsonProperty]
    public Cell league { get; protected private set; }
    [JsonProperty]
    public Cell flare_tnt { get; protected private set; }
    [JsonProperty]
    public Cell ritual { get; protected private set; }
    [JsonProperty]
    public Cell debuff { get; protected private set; }
    [JsonProperty]
    public Cell top { get; protected private set; }
    [JsonProperty]
    public Cell left { get; protected private set; }
    [JsonProperty]
    public Cell bott { get; protected private set; }
    [JsonProperty]
    public Cell right { get; protected private set; }
    [JsonProperty]
    public Cell buffs { get; protected private set; }
    [JsonProperty]
    public Cell party { get; protected private set; }
    [JsonProperty]
    public Cell chat { get; protected private set; }
    [JsonProperty]
    public Cell chat_help { get; protected private set; }
    [JsonProperty]
    public Cell menu { get; protected private set; }
    [JsonProperty]
    public Cell flask { get; protected private set; }
    [JsonProperty]
    public Cell xp_bar { get; protected private set; }
    [JsonProperty]
    public Cell ppa { get; protected private set; } //passive point available
    [JsonProperty]
    public Cell skills { get; protected private set; } //3E0
    protected V2 my_sp;
    Thread worker, center_updater;
    List<string> need_elem_nams = new() { "gui.ui_ppa", "gui.chat_box_elem?.up_arrow", "gui.ui_flask_root" };
    List<Element> need_init_well;
    public SafeScreen() {
        need_init_well = new() { ui.gui.ui_ppa, ui.gui.chat_box_elem?.up_arrow, ui.gui.ui_flask_root };
        Width = ui.game_window_rect.Width;
        Height = ui.game_window_rect.Height;
        top = new Cell(0, 0, Width, 10);
        left = new Cell(0, 0, 10, Height);
        bott = new Cell(0, Height - 10, Width, Height);
        right = new Cell(Width - 10, 0, Width, Height);
        var init_ok = true;
        int ni = 0;
        foreach (var e in need_init_well) {
            if (e == null || !e.IsValid) {
                init_ok = false;
                ui.AddToLog("SafeScreen init err for " + need_elem_nams[ni], MessType.Critical);
            }
            ni++;
        }
        if (!init_ok)
            return;
        GetLeagueFrame();

        var _ppa = ui.gui.ui_ppa.get_client_rectangle;
        ppa = new Cell(_ppa.X, _ppa.Y, _ppa.X + _ppa.Width, _ppa.Y + _ppa.Height);

        var chs = ui.gui.chat_box_elem.arrows.get_client_rectangle;
        chs.Inflate(3, 3);
        chat = new Cell(chs.X, chs.Y, chs.X + chs.Width, chs.Y + chs.Height);

        var chhe = ui.gui.ChatHelpPop.get_client_rectangle;
        chat_help = new Cell(chhe.X, chhe.Y, chhe.X + chhe.Width, chhe.Y + chhe.Height);

        var mbn = ui.gui.ui_menu_btn.get_client_rectangle;
        menu = new Cell(mbn.X, mbn.Y, mbn.X + mbn.Width, mbn.Y + mbn.Height);

        var flask_elem = ui.gui.ui_flask_root[0];
        var flr = new RectangleF();
        for (int i = 1; i < flask_elem.chld_count; i++) {
            if (i == 1)
                flr = flask_elem[i].get_client_rectangle;
            else
                flr = RectangleF.Union(flr, flask_elem[i].get_client_rectangle);
        }
        flr.Inflate(3, 3);
        flask = new Cell(flr.X, flr.Y, flr.X + flr.Width, flr.Y + flr.Height);
        var xp = ui.gui.ui_xp_bar.get_client_rectangle;
        xp_bar = new Cell(xp.X, xp.Y, xp.X + xp.Width, xp.Y + xp.Height);
        var sb = ui.gui.SkillBar.get_client_rectangle;
        sb.Inflate(10, 3);
        skills = new Cell(sb.X, sb.Y, sb.X + sb.Width, sb.Y + sb.Height);
        var wgs = ui.worldToGridScale;
        //CentrPoints.Add(("centre", new V2(27, 26) *wgs,  new V2(130, 80) *wgs));
        var x = 1.5f; var y = 0f;
        CentrPoints.Add(("C", new V2(x, y), new V2(x + 3f, y + 1.1f)));
        x = 1; y += 1.1f;
        CentrPoints.Add(("1", new V2(x, y), new V2(x + 3.6f, y + 1.5f)));
        x = 1; y += 1.5f;
        CentrPoints.Add(("2", new V2(x, y), new V2(x + 3.3f, y + 1.5f)));

        x = 0.5f; y = 0f;
        RoutPounts.Add(("0", new V2(x, y), new V2(x + 1f, y + 1f)));
        x = 0.5f; y -= 1f;
        RoutPounts.Add(("1", new V2(x, y), new V2(x + 1f, y + 1f)));
        for (int i = 0; i < 4; i++) {
            x += 1f;
            RoutPounts.Add(("r" + i, new V2(x, y), new V2(x + 1f, y + 1f)));
        }

        y += 1f;
        RoutPounts.Add(("t1", new V2(x, y), new V2(x + 1f, y + 1.2f)));
        y += 1.2f;
        RoutPounts.Add(("t2", new V2(x, y), new V2(x + 1f, y + 1.3f)));
        y += 1.5f; x -= 0.3f;
        RoutPounts.Add(("t3", new V2(x, y), new V2(x + 1f, y + 1.5f)));
        y += 1.5f;
        RoutPounts.Add(("t4", new V2(x, y), new V2(x + 1f, y + 1f)));
        for (int i = 0; i < 4; i++) {
            x -= 1.0f;
            RoutPounts.Add(("l" + i, new V2(x, y), new V2(x + 1f, y + 1f)));
        }
        x -= 0.2f; y -= 1.0f;
        RoutPounts.Add(("b", new V2(x, y), new V2(x + 1f, y + 1f)));
        for (int i = 0; i < 3; i++) {
            y -= 1.0f;
            RoutPounts.Add(("b" + i, new V2(x, y), new V2(x + 1f, y + 1f)));
        }

        center_updater = new Thread(() => {
            while (ui.b_running) {
                if (ui.gui == null) {
                    Thread.Sleep(1000);
                    continue;
                }
                if (ui.curr_state != gState.InGameState) { 
                    Thread.Sleep(ui.w8*10);
                    continue;
                }
                foreach (var p in CentrPoints) {
                    Centr[p.Item1] = new Cell(ui.me.gpos.Increase(p.Item2),
                        ui.me.gpos.Increase(p.Item3)) { b_block = true };
                }
                foreach (var p in RoutPounts) {
                    Rounts[p.Item1] = new Cell(ui.me.gpos.Increase(p.Item2),
                        ui.me.gpos.Increase(p.Item3)) { b_block = false };
                }
                Thread.Sleep(1000 / 60);
            }
        });
        center_updater.IsBackground = true;
        center_updater.Start();
        
        worker = new Thread(() => {
            while (ui.b_running) {
                if (ui.gui == null) {
                    Thread.Sleep(1000);
                    continue;
                }
                if (ui.curr_state != gState.InGameState) {
                    Thread.Sleep(ui.w8 * 10);
                    continue;
                }
                UpdateFrames();

                Blocks["top"] = top;
                Blocks["left"] = left;
                Blocks["bott"] = bott;
                Blocks["right"] = right;
                Blocks["buffs"] = buffs;
                Blocks["party"] = party;
                Blocks["chat"] = chat;
                Blocks["chat_help"] = chat_help;
                Blocks["menu"] = menu;
                Blocks["flask"] = flask;
                Blocks["xp_bar"] = xp_bar;
                Blocks["skills"] = skills;
                Blocks["ppa"] = ppa;
                Blocks["league"] = league;
                Blocks["ritual"] = ritual;
                Blocks["debuff"] = debuff;
                Blocks["flare_tnt"] = flare_tnt;
                Thread.Sleep(300);
            }
        });
        worker.IsBackground = true;
        worker.Start();
    }
    V2 old_buffs;
    int last_party_count = 0;
    bool b_last_league_state;
   
    protected void UpdateFrames() {
        var grc = ui.game_window_rect.Center();
        var top_elem = ui.gui.MyBuffPanel.get_client_rectangle;
        if (old_buffs.X != top_elem.Width || old_buffs.Y != top_elem.Height) {
            buffs = new Cell(0, 0, top_elem.Width, top_elem.Height);
            old_buffs = new V2() { X = top_elem.Width, Y = top_elem.Height };
        }

        var party_elem = ui.gui.party_panel;
        if (party_elem.members != null && party_elem.members.Count != last_party_count) {
            last_party_count = party_elem.members.Count;
            var face = new RectangleF();
            for (int i = 0; i < party_elem.members.Count; i++) {
                var p = party_elem.members[i];
                if (i == 0)
                    face = p.face_icon.get_client_rectangle;
                else
                    face = RectangleF.Union(face, p.face_icon.get_client_rectangle);
            }
            party = new Cell(face.X, face.Y, face.X + face.Width, face.Y + face.Height);
        }

        GetLeagueFrame();
        GetRitualeRevards();
        GetDebuffs();
        GetFlares();
    }
    bool b_last_flares_state;
    void GetFlares() {
        var flare = ui.gui.SkillBar.ui_flare_tnt;
        if (flare.Width > 0) {
            if (!b_last_flares_state) {
                b_last_flares_state = true;
                var rect = ui.gui.SkillBar.ui_flare_tnt.get_client_rectangle;
                flare_tnt = new Cell(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
            }

        }
        else {
            if (b_last_flares_state) {
                flare_tnt = new Cell(0, 0, 0, 0);
                b_last_flares_state = false;
            }
        }
    }

    bool b_last_debuf_state;
    void GetDebuffs() {
        var rr = ui.gui.debuffs_pannel;
        if (rr.IsVisible) {
            if (!b_last_debuf_state) {
                b_last_debuf_state = true;
                var rect = rr.get_client_rectangle;
                debuff = new Cell(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
            }

        }
        else {
            if (b_last_debuf_state) {
                debuff = new Cell(0, 0, 0, 0);
                b_last_debuf_state = false;
            }
        }
    }
    bool b_last_ritual_state;
    void GetRitualeRevards() {
        var rr = ui.gui.ui_ritual_rewards;
        if (rr.IsVisible) {
            if (!b_last_ritual_state) {
                b_last_ritual_state = true;
                var rect = rr.get_client_rectangle;
                ritual = new Cell(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
            }
        }
        else {
            if (b_last_ritual_state) {
                b_last_ritual_state = false;
                ritual = new Cell(0, 0, 0, 0);
            }
        }
    }
    void GetLeagueFrame() {
        var lam = ui.gui.ui_lake_map;
        if (lam.IsVisible) {
            if (!b_last_league_state) {
                b_last_league_state = true;
                var lamr = lam.get_client_rectangle;
                league = new Cell(lamr.X, lamr.Y, lamr.X + lamr.Width, lamr.Y + lamr.Height);
            }

        }
        else {
            if (b_last_league_state) {
                league = new Cell(0, 0, 0, 0);
                b_last_league_state = false;

            }
        }
    }

    public bool b_gp_Inside_centr(V2 gp) {
        foreach (var b in Centr) {
            if (b.Value.Insade(gp)) {
                return true;
            }
        }
        return false;
    }
    public bool SetMouseBestCentrSP(V2 gp, aTask from) {
        ui.AddToLog("SetMouseBestCentrSP not implemented", MessType.Error);
        //var sorted = Rounts.OrderBy(r => r.Value.center.GetDistance(ui.me.gpos)).ToArray();
        //foreach (var b in sorted) {
        //    if (ui.nav.b_can_hit(b.Value.center)) {
        //        gp = b.Value.center;
        //        var sp = ui.TgpToSP(gp);
        //        Mouse.SetCursor(sp, "SetMouseBestCentrSP", 3, false);
        //        return true;
        //    }
        //}
        //from.b_debug = true;
        //ui.tasker.Hold("SetMouseBestCentrSP"); //prevent server disconnect
        //ui.sound_player.PlaySound(@"I hit a wall, my God.mp3"); //@"C:\Sounds\Help me.mp3"
        return false;
    }
}
