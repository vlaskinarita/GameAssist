using ImGuiNET;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Stas.GA;
/// <summary>
///     Reads and stores the global states of the game.
/// </summary>
[CodeAtt("game ctate changer")]
public class GameStates : RemoteObjectBase {
    internal GameStates(IntPtr address) : base(address) {
    }
    gState _cgs = gState.GameNotLoaded; //current game state
    gState _old_gs;
    public gState curr_gState {
        get => _cgs;
        private set {
            if (_cgs != value) {
                _old_gs = _cgs;
                _cgs = value;
                if (_old_gs == gState.LoadingState && _cgs == gState.InGameState) {
                }
            }
        }
    }
    IntPtr curr_gState_ptr = IntPtr.Zero;
    GameStateStaticOffset myStaticObj;
    /// <summary>
    /// set false if game closed
    /// </summary>
    public bool b_ready = false;
    GameStateOffset data = default;
    internal override void Tick(IntPtr ptr, string from = null) {
        Address = ptr; ////00007ff6f32bed30
        if (Address == IntPtr.Zero) {//from game.watcher close
            Clear();
            return;
        }
        if (!b_ready) {//init from game_watcher - one time per game instance
            myStaticObj = ui.m.Read<GameStateStaticOffset>(Address);
            data = ui.m.Read<GameStateOffset>(myStaticObj.GameState);
            AllStates[data.State0] = 0;
            AllStates[data.State1] = (gState)1;
            AllStates[data.State2] = (gState)2;
            AllStates[data.State3] = (gState)3;
            AllStates[data.State4] = (gState)4;
            AllStates[data.State5] = (gState)5;
            AllStates[data.State6] = (gState)6;
            AllStates[data.State7] = (gState)7;
            AllStates[data.State8] = (gState)8;
            AllStates[data.State9] = (gState)9;
            AllStates[data.State10] = (gState)10;
            AllStates[data.State11] = (gState)11;
            b_ready = true;
        }
        var tik_gState_ptr = ui.m.Read<IntPtr>(ui.m.Read<StdVector>(ui.m.Read<IntPtr>(Address) + 8).Last - 0x10);// Get 2nd-last ptr.
        if (tik_gState_ptr != default && tik_gState_ptr != curr_gState_ptr) {
            //here game state cahnge event
            curr_gState_ptr = tik_gState_ptr;
            //TODO this hapened obly ones, mb same time was cleare()
            Debug.Assert(AllStates.ContainsKey(curr_gState_ptr));
            curr_gState = AllStates[curr_gState_ptr];
            ui.AddToLog(tName + ".curr_gState=>" + _cgs, MessType.Warning);
            if (curr_gState != gState.EscapeState && curr_gState != gState.InGameState) {
                ingame_state.Tick(default);
                ui.ResetWorker();
                ui.nav.b_ready = false; //dont check hero and visited
                ui.gui.Tick(default, tName + "=>bad stage");
            }
        }
        Debug.Assert(data.State0 != default && data.State4 != default);

        if (curr_gState == gState.InGameState) {
            area_loading_state.Tick(data.State0, tName);
            ingame_state.Tick(data.State4, tName);
        }
    }
    protected override void Clear() {
        b_ready = false; //debug here from
        myStaticObj = default;
        curr_gState_ptr = default;
        curr_gState = gState.GameNotLoaded;
        AllStates.Clear();
        area_loading_state.Tick(default, tName + ".Cleare");
        ingame_state.Tick(default);
    }
    /// <summary>
    ///     Gets a dictionary containing all the Game States addresses.
    /// </summary>
    public Dictionary<IntPtr, gState> AllStates { get; } = new();
    /// <summary>
    /// [1] ui.states.area_loading_state 
    /// </summary>
    public AreaLoadingState area_loading_state { get; } = new(default);
    /// <summary>
    ///  [2] ui.states.ingame_state 
    /// </summary>
    public InGameState ingame_state { get; } = new(default);
    internal override void ToImGui() {
        base.ToImGui();
        if (ImGui.TreeNode("All States Info")) {
            foreach (var state in AllStates) {
                ImGuiExt.IntPtrToImGui($"{state.Value}", state.Key);
            }
            ImGui.TreePop();
        }
        ImGui.Text($"Current State: {curr_gState}");
    }
}