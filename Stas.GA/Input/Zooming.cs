using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using V2 = System.Numerics.Vector2;
using V3 = System.Numerics.Vector3;
namespace Stas.GA;

public partial class InputChecker {
    void Zooming() {
        if (ui.sett.b_use_gh_map) {
            //if (Keyboard.IsKeyDown(Keys.NumPad5, "ICh")) {
            //    ui.map_offset = V2.Zero;
            //}
            if (Keyboard.IsKeyDown(Keys.NumPad8, "ICh")) {
                ui.map_offset.Y += 0.1f;
            }
            if (Keyboard.IsKeyDown(Keys.NumPad2, "ICh")) {
                ui.map_offset.Y -= 0.1f;
            }
            if (Keyboard.IsKeyDown(Keys.NumPad4, "ICh")) {
                ui.map_offset.X += 0.1f;
            }
            if (Keyboard.IsKeyDown(Keys.NumPad6, "ICh")) {
                ui.map_offset.X -= 0.1f;
            }
            if (Keyboard.IsKeyDown(Keys.NumPad7, "ICh")) {
                ui.sett.map_angle = ui.sett.map_angle += 0.05f;
            }
            if (Keyboard.IsKeyDown(Keys.NumPad9, "ICh")) {
                ui.sett.map_angle = ui.sett.map_angle -= 0.05f;
            }
            if (Keyboard.IsKeyDown(Keys.NumPad1, "ICh zoom_in")) {
                ui.sett.map_scale = Math.Clamp(ui.sett.map_scale += 0.01f, 0.5f, 20);
            }
            if (Keyboard.IsKeyDown(Keys.NumPad3, "ICh zoom_out")) {
                ui.sett.map_scale = Math.Clamp(ui.sett.map_scale -= 0.01f, 0.5f, 20);
            }
        }
        else {
            if (ui.curr_map.danger > 0) {
                ui.sett.map_scale = ui.sett.map_scale_def;
            }
            else {
                if (ui.sett.b_use_keybord_for_zoom) {
                    if (Keyboard.IsKeyDown(ui.sett.zoom_in, "ICh zoom_in")) {
                        ui.sett.map_scale = Math.Clamp(ui.sett.map_scale += 0.01f, 0.5f, 20);
                    }
                    if (Keyboard.IsKeyDown(ui.sett.zoom_out, "ICh zoom_out")) {
                        ui.sett.map_scale = Math.Clamp(ui.sett.map_scale -= 0.01f, 0.5f, 20);
                    }
                }
                else {
                    if (Mouse.IsButtonDown(Keys.XButton1)) {
                        ui.sett.map_scale = Math.Clamp(ui.sett.map_scale += 0.2f, 0.5f, 20);
                    }
                    if (Mouse.IsButtonDown(Keys.XButton2)) {
                        ui.sett.map_scale = Math.Clamp(ui.sett.map_scale -= 0.2f, 0.5f, 20);
                    }
                }
            }
        }
    }
}
