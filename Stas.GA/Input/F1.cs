//using Stas.POE.Core;
using SharpDX.DXGI;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using V2 = System.Numerics.Vector2;
namespace Stas.GA;
public partial class InputChecker {

    void F1() {
        if (Keyboard.b_Try_press_key(Keys.F1, "Input checker", 500, true)) {
            //ui.sett.b_debug = true;
            if (ui.b_alt) {
                //ui.test.uiElementFinder();
                //ui.test.GetTopElemUnderCursor();
                //ui.test.GetRootElemUnderCursor();
                //ui.test.WorlToSPCheck();
               
            }
            else {
                var flask = ui.flasks[0, 0];
                //var cam = ui.m.Read<CameraOffsets>( ui.camera.Address);
                //ui.test.FindUiElemNotUnick("Nessa");
                //ui.test_elem =FindSameUIElement("Decorations", 1)[0].Item1; //660
                //ui.test_elem =FindSameUIElement("Inventory",1)[0].Item1; //0x568
                //ui.test_elem = ui.gui.large_map;
                #region OLD
                //var elem = new Element("test");
                //ui.texts.Clear();
                //elem.Tick(new nint(0x234DE439790));


                //GetEntComponetns();
                //ui.me.GetComp<Positioned>(out var pos);
                //var skill = ui.worker.jump.skill.CanBeUsed;
                //ui.test_elem = ui.gui;
                //var stats = ui.me.Stats;
                //b_busy_info_crash();
                //var b = ui.gui.b_busy;
                //var bi = ui.gui.b_busy_info;
                //ui.test_elem = ui.gui;
                //CompareActor();
                //var ea = ui.gc?.entity_list.Entities.OrderBy(x => x.gdist_to_me).ToList();
                //ui.test_elem = ui.gui.chat_box_elem;
                //var mess = ui.gui.chat_box_elem.messages;
                //ui.test_elem = ui.gui.player_inventory;
                //ui.test.Inventry();
                //ui.test.GetElemUnderCursor(2); //540
                //ui.test.CheckBuffType();
                //ui.test.Render();
                //var old_skill = old_actor.ActorSkills[10];

                //var sw = skill.EffectsPerLevel.SkillGemWrapper; 
                //var bufs = ui.me.buffs;
                //ui.me.GetComp<Actor>(out var actor);
                //var skills = actor.actor_skills;
                //TestSkill();//+
                //var curr_map_name = ui.curr_map_name;
                //var cmn2 = ui.states.area_loading_state.CurrentAreaName;
                #endregion
            }
        }
    }
    void TestCamera() {
        var ptr = ui.camera.Address;
        var co = new CameraOffsets();
        Camera.GetCameraOffsets(ptr, ui.states.ingame_state.Address, ref co);
    }
    //run after Elem with pattern was open and closed
    List<(Element, string)> FindSameUIElement(string pattern, int index) {
        var res = new List<(Element, string)>();
        foreach (var e_ptr in ui.gui.children_pointers) {
            var ne = new Element(e_ptr, e_ptr.ToString("X"));
            ne.Tick(e_ptr, "F1.FindSameUIElement"); //for reload children
            if (ne.children.Count > index+1) {
                if (ne.children[index].Text == pattern)
                    res.Add((ne, get_offs_to_gui(e_ptr)));
            }
        }
        string get_offs_to_gui(IntPtr ptr_i_need) {
            string res = "";
            var gui_ptr = ui.gui.Address;
            for (var i = 0; i < 1600; i += 8) {
                var c_ptr = ui.m.Read<IntPtr>(gui_ptr + i);//current ptr
                if (c_ptr == ptr_i_need) {
                    res = i.ToString("X");
                    break;
                }
            }
            return res;
        }
        return res;
    }
    void GetEntComponetns() {
        ui.me.GetComp<Actor>(out var actor);
       
        var start = actor.Address;
        var list = new Dictionary<IntPtr, string>();
        for (int off = 0; off < 64; off += 8) {
            var ne = new Entity(actor.Address + off);
            var entityData = ui.m.Read<EntityOffsets>(ne.Address);
           
            var idata = entityData.ItemBase;
            var entityComponent = ui.m.ReadStdVector<IntPtr>(idata.ComponentListPtr);
            var entityDetails = ui.m.Read<EntityDetails>(idata.EntityDetailsPtr);
            var lookupPtr = ui.m.Read<ComponentLookUpStruct>(entityDetails.ComponentLookUpPtr);

            var namesAndIndexes = ui.m.ReadStdBucket<ComponentNameAndIndexStruct>(lookupPtr.ComponentsNameAndIndex);
            Debug.Assert(namesAndIndexes.Count < 20);
            for (var i = 0; i < namesAndIndexes.Count; i++) {
                var nameAndIndex = namesAndIndexes[i];
                if (nameAndIndex.Index >= 0 && nameAndIndex.Index < entityComponent.Length) {
                    var name = ui.m.ReadString(nameAndIndex.NamePtr);
                    if (!string.IsNullOrEmpty(name)) {
                        //list.Add(name, entityComponent[nameAndIndex.Index]);
                    }
                }
            }
            Thread.Sleep(1);
            ui.AddToLog("sershing...[" + off + "]", MessType.Critical);
        }
    }

    void TestSkill() {
        ui.me.GetComp<Actor>(out var actor);
        var skill = actor.actor_skills.FirstOrDefault(s => s.Id == 32772);
        var @in = skill.InternalName;
        var name = skill.Name;
    }
    void b_busy_info_crash() {
        var tl = new List<int>();
        int i = 0;
        for (; ; ) {
            var check = ui.b_busy;
            var value = ui.gui.b_busy_info;
            var info = "i=[" + (i++) + "] size=[" + value.Length + "] v=" + value;
            tl.Add(value.Length);
            ui.AddToLog(info);
        }
    }
    //void ComareCam() {
    //    var old_cam = ui_loader.ui.gc.IngameState.Camera;
    //    var cam = ui.camera;
    //    Debug.Assert(cam.Address == old_cam.ptr);
    //}
    //void CompareActor() {
    //    ui.me.GetComp<Actor>(out var actor);
    //    ui_loader.ui.me.GetComp<ExileCore.PoEMemory.Components.Actor>(out var old_actor);
    //    for (int i = 8; i < old_actor.ActorSkills.Count; i++) {
    //        var os = old_actor.ActorSkills[i];//old
    //        var cs = actor.actor_skills[i];//current
    //        Debug.Assert(os.ptr == cs.Address);
    //        Debug.Assert(os.EffectsPerLevel.ptr == cs.EffectsPerLevel.Address);
    //        Debug.Assert(os.EffectsPerLevel.SkillGemWrapper.ActiveSkill.ptr
    //                        == cs.EffectsPerLevel.SkillGemWrapper.ActiveSkill.Address);
    //    }
    //}
}