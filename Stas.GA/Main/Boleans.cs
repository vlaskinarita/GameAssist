#region using
using System;
using System.Collections.Generic;
using System.Linq;
using V2 = System.Numerics.Vector2;
using V3 = System.Numerics.Vector3;
#endregion

namespace Stas.GA {
    public partial class ui {
        public static bool b_i_see_safe_ent_pos(V3 pos) {
            var sp = WorldTPToSP(pos);
            return b_sp_is_safe(sp);
        }
        public static bool b_can_safe_hit_ent(Entity ent) {
            var isee = b_i_see_safe_ent_pos(ent.pos);
            var can_hit = nav.b_can_hit(ent);
            return isee && can_hit;
        }
        public static bool b_running = true, b_draw_bots = true, b_pause, b_tile, b_ga_menu_top,  b_appl_started, b_minimize, b_show_info_over,
            b_vs, b_edit_sett, b_show_cell,  b_in_mine, b_only_unknow, b_draw_bad_centr, b_draw_save_screen;
        public static bool b_trade_top { get; private set; }
        //public bool b_modal => gui == null ? false : gui.Ultimatum.IsVisible || gui.modal_dialog.IsVisible || gui.esc_dialog.IsVisible;
       
        public static bool b_ingame { get { 
                return curr_state == gState.InGameState; 
            } } 
        public static bool b_m_centered;
        public static bool b_trader_top { get; private set; }

        public static bool b_shift => Keyboard.IsKeyDown(Keys.ShiftKey);
        public static bool b_contr_shift => b_contrl && b_shift;
        public static bool b_alt_shift => b_alt && b_shift;
        public static bool b_contr_alt => b_contrl && b_alt;
        public static bool b_alt {
            get {
                var alt = Keyboard.IsKeyDown(Keys.Menu);
                //if (alt && !b_m_centered && !b_ui_busy && !b_contrl && b_top) {
                //    b_m_centered = true;
                //    var rect = EXT.GetWindowRectangle(poe_proc.MainWindowHandle);
                //    Mouse.SetCursor(new V2(rect.Width / 2, rect.Height / 2), "Centering after Alt down");
                //    AddToLog("alt pressed=[" + (alt_pressed += 1) + "]");
                //} 
                //if(!alt)   b_m_centered = false;
                return alt;
            }
        }
        public static bool b_grace {
            get {
                var buffs = me.buffs;
                if (buffs == null)
                    return false;
                return buffs.StatusEffects.ContainsKey("grace_period");
            }
        }

        public static bool b_contrl => Keyboard.IsKeyDown(Keys.ControlKey);
        public static bool b_I_died => me == null ? false : me.IsDead;
        public static bool b_game_top {
            get {
                var res = curr_top_ptr == game_ptr;
                //ui.warning=("game_top=[" + res + "]");
                return res;
            }
        }
        public static bool b_imgui_top {
            get {
                if (DrawMain.scene == null)
                    return false;
                var sdl_ptr = DrawMain.scene.sdl_window.GetHWnd();
                return curr_top_ptr == sdl_ptr;
            }
        }
        public static bool b_busy {
            get {
                if (gui ==null || curr_state != gState.InGameState)
                    return true;
                var res = b_chat_box_inp || gui.b_busy;
                //var info = gui.b_busy_info;
                //ui.warning = "open_right_panel=" + ui.gui.open_right_panel.IsVisible;
                return res;
            }
        }
        public static bool b_chat_box_inp {
            get {
                var e = gui.chat_box_elem;
                return e == null || e.input == null? false : e.input.IsVisible;
            }
        }
        static DateTime last_mine_check_time;
        static bool last_mine_val;
        public static bool b_mine_town {
            get {
                if (curr_world.world_area.Name == "Azurite Mine") {
                    if (last_mine_check_time.AddSeconds(1) < DateTime.Now) {
                        last_mine_check_time = DateTime.Now;
                        var stash = curr_map.static_items.Values.FirstOrDefault(i=>i.m_type == miType.Stash);
                        if (stash != null && stash.ent.IsValid && stash.gdist_to_me < 250)
                            last_mine_val = true;
                        else
                            last_mine_val = false;
                    }
                    return last_mine_val;
                }
                return false;
            }
        }
        /// <summary>
        /// not enemy here and can't use any skills here at all
        /// </summary>
        public static bool b_town { get {
                var is_town = curr_world.world_area.IsTown;
                var roug = curr_world.world_area.Name == "The Rogue Harbour";
                return is_town || roug;
            } }  
        /// <summary>
        /// No enemy here but we can cast same buff mb
        /// </summary>
        public static bool b_home { get {
                var ho = curr_world.world_area.IsHideout;
                return ho || b_mine_town;   } }  
    }
}
