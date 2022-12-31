using ImGuiNET;
using V2 = System.Numerics.Vector2;
using System.Drawing;
using V3 = System.Numerics.Vector3;
using sh = Stas.GA.SpriteHelper;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace Stas.GA;
partial class DrawMain {
    public ConcurrentDictionary<string, RectangleF> frames = new ();

    int max_frames_same_time = 250;
    bool b_was_copy = false;
    bool b_relative = true;
    Dictionary<long, string> knows_adress = new Dictionary<long, string>();
    Dictionary<long, string> calc = new Dictionary<long, string>();
    bool draw_children = true;
    int selected_elem = 0;
    bool b_was_selected;
    /// <summary>
    /// here we will draw all debug frames
    /// </summary>
    ImDrawListPtr ui_elem_ptr;
    Action DrawZeroElem;
    void GetUiFrames() {
        DrawZeroElem = null;

        b_was_copy = false;
        selected_elem = 0;
        b_was_selected = false;
        ImGui.Begin("Same UI Elements...",
            ImGuiWindowFlags.HorizontalScrollbar
            | ImGuiWindowFlags.AlwaysVerticalScrollbar);
        ui_elem_ptr = ImGui.GetWindowDrawList();
        ImGui.PushStyleColor(ImGuiCol.Button, Color.DarkMagenta.ToImgui());
        ImGui.Button("This frame automatically focuses.\nTo cancel autofocus - press [Alt]");
        ImGui.PopStyleColor();
        ImGui.Checkbox("draw children", ref draw_children);

        ImGui.SameLine();
        if (ImGui.Checkbox("this on top", ref ui.sett.b_gui_debug_on_top)) {
            ui.sett.Save();
        }
        ImGuiExt.ToolTip("draw this frame( with ui elements) on top always");
        ImGui.SameLine();

        if (ImGui.Button("close")) {
            ui.SetDebugPossible(() => {
                ui.test_elem = null;
            });
           
        }
        ImGui.Button("Frames=" + frames.Count + " from:" + max_frames_same_time);

        frames.Clear();//we need clear AFTER debug frames count^^
        ImGui.TreeNode("UI root=" + ui.test_elem?.Address.ToString("X"));
        AddToTree(ui.test_elem);
        ImGui.TreePop();

        ImGui.End();
    }
    void DrawUiFrames() {
        lock (frames) {
            foreach (var f in frames.Values) {
                var lt = new V2(f.Left, f.Top) / ui.screen_k + ui.w_offs;
                if (f.Width != 0 || f.Height != 0) {
                    ui_elem_ptr.AddRect(lt, (new V2(f.Right, f.Bottom) / ui.screen_k) + ui.w_offs,
                        Color.Yellow.ToImgui(), 0, 0x0, 0);
                }
                DrawZeroElem?.Invoke();
            }
        }
    }
    void AddToTree(Element el) {
        var gui_offs = "";
        var text = "";
        if (el == null || !el.IsValid) {
            ui.AddToLog("AddToTree err: bad element", MessType.Error);
            return;
        }
        if (!calc.ContainsKey(el.Address)) {//calc offset at game_ui
            gui_offs = GetOffs(ui.gui.Address, el.Address);
            calc[el.Address] = gui_offs;
        }
        else {
            gui_offs = calc[el.Address];
        }

        if (el.chld_count == 0) {
            text = el.Text;
            if (text?.Length > 32)
                text = text.Substring(0, 32) + "...";
        }
        else {
            text = "[" + el.chld_count + "]";
        }

        var adress = $"{el.Address:X}";
        if (gui_offs.Length > 0 && b_relative) {
            adress = "0x" + gui_offs;
        }
           
        if (ImGui.TreeNode($"{adress} {text}")) {
            _CheckAndAddFrame();//we need check this too, с
            foreach (var ptr in el.children_pointers) {
                if (!ui.elements.ContainsKey(ptr)){
                    ui.elements[ptr] = new Element(ptr, "debug gui"); }
                AddToTree(ui.elements[ptr]);
            }
            ImGui.TreePop();
        }
        _CheckAndAddFrame();

        void _CheckAndAddFrame() {
            if (ImGui.IsItemHovered(ImGuiHoveredFlags.RootWindow) && !b_was_selected) {
                selected_elem += 1;
                b_was_selected = true;
                AddFrames(el);
                if (!b_was_copy && ImGui.IsMouseClicked(ImGuiMouseButton.Right)) {
                    var str = "";
                    if (el.Text != null && el.Text.Length > 0)
                        str = " " + el.Text;
                    ui.SetDebugPossible(() => {
                        var res = el.Address.ToString("X") + str;
                        ImGui.SetClipboardText(res);
                        ui.AddToLog("Cliked on ui.elem=" + res);
                    });
                    b_was_copy = true;
                }
            }
        }
    }

    string GetOffs(long start, long addr_i_need) {
        var res = "";
        var n = 0;
        for (var i = start; i < start + 0x8000; i += 8) {
            var addr = ui.m.Read<long>(i);
            //var ne = GetObject<Element>(addr);
            //if(addr > 0 && ne.IsValid) {
            //    res = (i - start).ToString("X"); //B8 //118 //298 //2B8
            //    direct[res]=ne;
            //    Element root = ne;
            //    while(root.Parent != null) {
            //        root = root.Parent;
            //    }
            //    var b_root_have_link_to_game_ui = root.Children.FirstOrDefault(ch => ch.Address == game_ui.Address) != null;
            //    if(b_root_have_link_to_game_ui) {
            //       // ui.AddToLog("Found ui_root at: " + res);
            //    }
            //    root.IngameStateOffsets_offs = res;
            //    roots[root.Address.ToString("X")] = root;
            //}
            if (addr == addr_i_need) {
                res = (i - start).ToString("X");
                break;
            }
            n += 1;
        }
        return res;
    }
    public void AddFrames(Element root) {
        if (frames.Count < max_frames_same_time) {
            var rect = root.get_client_rectangle;
            var his = 12;
            frames[root.Address.ToString("X")] = rect;
            var lt = new V2(rect.Left, rect.Top) / ui.screen_k + ui.w_offs;
            if ((rect.Width == 0 || rect.Height == 0) && DrawZeroElem == null) {
                DrawZeroElem = delegate () {
                    var uv = sh.GetUV(MapIconsIndex.FrameArrow);
                    ui_elem_ptr.AddImage(icons, lt.Increase(-his, -his), lt.Increase(his, his),
                        new V2(uv.Left, uv.Top), new V2(uv.Left + uv.Width, uv.Top + uv.Height));
                };
            }
        }

        else
            return;
        if (draw_children) {
            var cha = root.children.ToArray();
            foreach (var ch in cha) {
                //Debug.Assert(frames.Count < max_frames_same_time + 1);
                AddFrames(ch);
            }
        }
    }

}
