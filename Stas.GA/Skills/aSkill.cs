using Newtonsoft.Json;
using System.Diagnostics;
using V2 = System.Numerics.Vector2;
using V3 = System.Numerics.Vector3;
namespace Stas.GA;

[JsonObject(MemberSerialization.OptIn)]
public abstract class aSkill {
    public string tName => GetType().Name;
    [JsonIgnore]
    public aTask parent_task { get; set; }
    public bool b_can_pull { get; protected private set; } = false;
    public bool b_can_hit { get;  protected private set; } = true; //for distroy a barall etc
    public bool b_melee_aoe { get; protected private set; } = false;//use for debuff/protect melee radius
    /// <summary>
    /// setup mouse position BEFor use this. Last targeted get from Mouse.last_targeted
    /// </summary>
    public bool b_need_target { get; protected private set; } = false;
    public DateTime last_use { get; protected private set; } = DateTime.MinValue; //same like "cast start time" for chaneling
    public bool b_debug = true;
    //public bool b_must_stop => DateTime.Now > last_use.AddMilliseconds(using_time);
    public bool b_started => Keyboard.IsKeyDown(key);
    public Entity me => ui.me;
    protected Random R = new Random();
    public int using_time { get; protected private set; }
    public Keys key { get; protected private set; }
    public int id { get; }
    public int cooldown { get; protected private set; }
    public int mana_cost { get; protected private set; } = 5;
    public int cast_time { get; protected private set; }
    public int grange { get; protected private set; } = 6;
    public Skill skill;
    public string name;
    public virtual bool b_must_stop { get; set; } = false;
    /// <summary>
    /// 99% its ui.b_alt || ui.mapper.danger == 0
    /// </summary>
    public Func<bool> MustBeStop;
    public aSkill(Keys _key, int _grange, int _mana_cost, int _cast_time, int _cooldown) {
        key = _key;
        grange = _grange;
        mana_cost = _mana_cost;
        cast_time = _cast_time;
        cooldown = _cooldown;
        id = ui.min_new_skill_id += 1;
        MustBeStop = DefaultMustBeStop;
    }
    bool DefaultMustBeStop() {
        if (ui.b_alt || ui.curr_map.danger == 0)
            return true;
        return false;
    }
    public float radius { get; protected private set; } = 20;

    //TODO: here must be stop moving altime
    [Obsolete]
    public void SetOnTgp(V2 tgp, bool stop_moving=true, bool debug = false) {
        var sp = ui.TgpToSP(tgp);
        sp = ui.SpToSp_safe(sp);
        Mouse.SetCursor(sp, tName, 3, stop_moving); 
    }
    public void SetOnTP(V3 tp, bool stop_moving=true, bool debug=false) {
        if (!ui.b_game_top) {
            ui.AddToLog("SetOnTP err: NOT b_game_top", MessType.Error);
            return;
        }
        var sp = ui.WorldTPToSP(tp);
        sp = ui.SpToSp_safe(sp);
        Mouse.SetCursor(sp, tName, 3, stop_moving, true, 600);
    }

    public virtual void Reset() {
        ///last_use = DateTime.MinValue;
        started.Clear();
        stopped.Clear();
    }
    public abstract void Run(Action do_after = null);
     

    /// <summary>
    /// skill using range in greed points
    /// </summary>
    public virtual bool b_on_cooldown {
        get {
            if(last_use == DateTime.MinValue || cooldown ==0 )
                return false;
            var cd_factor = Math.Max(cooldown, using_time);
            return last_use.AddMilliseconds(cd_factor) > DateTime.Now;
        }
    }
   
    public virtual bool b_ready {
        get {
            if (ui.me.Address==IntPtr.Zero ||ui.b_town) {
                //ui.AddToLog(tname+" NOT b_ready ... ", MessType.Error);
                return false;
            }
            var b_mana = false;
            if (ui.me.GetComp<Life>(out var life) && life != null)
                b_mana= life.Mana.Current >= mana_cost;

            return   b_mana && !b_on_cooldown  ;
        }
    }
    public override string ToString() {
        return tName + " ready=" + b_ready + " oncd=" + b_on_cooldown;
    }
    
    public void WasUsedManual() {
        //ui.tasker.UpdateLastManualUsing(this);
        //ui.AddToLog(tname + " was used manual");
    }
    DateTime last_started, last_stopped;
    int warning_fast_reuse = 200;
    List<double> started = new();
    public void StartCast() {
        if (!ui.b_game_top) { //dont start cast  in order not to spoil the code if we are in debug mode via visual studio
            ui.AddToLog("Cant't use a skill... NOT b_top");
            return;
        }
        if (ui.life.Mana.CurrentInPercent < 40) {
            if (!ui.tasker.UseManaFlask())
                ui.AddToLog(name + " w8ting mana...", MessType.Warning);
            return;
        }
        if (!Keyboard.IsKeyDown(key)) {
            if (last_started != DateTime.MinValue) {//this below is just for debugging
                var now = DateTime.Now;
                var elaps = (now - last_started).TotalMilliseconds;
                var oncd = this.b_on_cooldown;
                if (elaps < warning_fast_reuse) {
                    started.Add(elaps);
                    //Debug.Assert(stopped.Count < 30);
                    ui.AddToLog(parent_task + "=>" + tName + ".. last started=[" + elaps + "]", MessType.Warning);
                }
            }
            Keyboard.KeyDown(key, parent_task + "=>" + tName + " KeyDown");
            last_started = DateTime.Now; //TODO for debug only(winter orb bug)
            last_use = DateTime.Now;
        } else {//wtop and debug here  from called - it's bad
            ui.AddToLog("StartCast err: already casting", MessType.Error);
        }
    }
  
    List<double> stopped = new();
    public void StopCast() {
        if(b_started) { 
            if (last_stopped != DateTime.MinValue) { // => here same debug part
                var elaps = (DateTime.Now - last_stopped).TotalMilliseconds;
                //var stage = (this as WinterOrb)?.stage;
                if (elaps < warning_fast_reuse) {
                    stopped.Add(elaps);
                    ui.AddToLog(parent_task + "=>"+tName + ".. last stopped=[" + elaps + "]", MessType.Warning);
                    //Debug.Assert(stopped.Count < 50);
                    if (stopped.Count > 50)
                        stopped.Clear();
                }
            }
            last_stopped = DateTime.Now;//first time init here
            Keyboard.KeyUp(key, parent_task + "=>" + tName + " KeyUp");
        }
    }

    public void OneHit(Action do_after=null) {
        Keyboard.KeyPress(key);
        last_use = DateTime.Now;
        do_after?.Invoke();
    }
    public void LikeChannelling( Action do_after=null) {
        Debug.Assert(cast_time > 0);
        if (ui.b_alt || MustBeStop()) {
            StopCast();
            if (ui.b_alt) {
                ui.AddToLog(tName + " LikeChannelling stopped by ALT", MessType.Warning);
            }//debug here mb? when alt is pressed this should not be called
            do_after?.Invoke();
            return;
        }
        if (!b_started) {

            StartCast();
            last_use = DateTime.Now;
            return;
        }
    }
    public void HittingUntilNeedStop( Action do_after) {
        if(last_use==DateTime.MinValue)
            StartCast();
        if (b_must_stop || ui.b_alt) { StopCast();
            do_after?.Invoke();
        }
    }
}
