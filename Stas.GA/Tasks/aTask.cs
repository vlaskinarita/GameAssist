using Newtonsoft.Json;

namespace Stas.GA;

public abstract class aTask {
    public string ttype_full => GetType().Name;
    public float gdist_i_need = 12;
    public bool b_need_auto_stop { get; protected private set; } = false;
    public aTask parent;
    public bool b_try_close_gp { get; protected private set; } = false;
    public virtual string short_name { get; }
    public bool b_danger_stop { get; protected private set; } = true;
    public bool b_ignore_ui_busy;
    public virtual bool b_alt_stop => b_debug; //set false for debug only
                                               //most of the tasks are performed only inside the game.Exceptions: login and hero selection
    public bool b_ingame { get; protected private set; } = true;
    public bool b_auto_top = true;
    public bool b_dont_w8_grace = true;
    public DateTime started { get; } //started time for debug
    public int ticks = 0; //elapsed tick for debug
    public string elapsed_second => (DateTime.Now - started).TotalSeconds.ToRoundStr(3);
    TaskErrors _error;
    [JsonIgnore]
    public TaskErrors error {
        get { return _error; }
        set {
            _error = value;
        }
    }
    string _le;
    public string last_error { get { return _le == null ? error.ToString() : _le; } set { _le = value; } }
    public bool b_done = false;
    public Action do_after;
    protected Func<bool> do_if;
    public bool b_need_leader = false;
    /// <summary>
    /// spamming to log and using for wisual debug too
    /// </summary>
    public bool b_debug = false;
    [JsonIgnore]
    public virtual string id_name => id + ".." + (short_name == null ? GetType().Name : short_name);
    public virtual string tname => short_name == null ? GetType().Name : short_name;

    public uint id { get; }
    [JsonIgnore]
    public Entity me => ui.me;
    public int max_try_count = 30;//max stuck tick, You need the minimum possible value with the current ping
    public int try_count => try_from.Count;
    public List<string> try_from = new List<string>();
    public void AddTryCount(string from) {
        try_from.Add(from);
        if (b_debug)
            ui.AddToLog(id_name + ".. " + from + "[" + try_count + "]", MessType.Warning);
    }
    public void CleareTryCount() {
        try_from.Clear();
    }
    /// <summary>
    ///have we danger right now
    /// </summary>
    public abstract void Do();
}
public class ChannelingUntilCondition : aTask {
    Func<bool> condition;
    public override string short_name => "ChUC";
    aSkill skill;
    public ChannelingUntilCondition(aSkill _skill, Func<bool> _condition) {
        condition = _condition;
        skill = _skill;
    }

    public override void Do() {
        if (condition.Invoke()) {
            ui.tasker.TaskDone(this, "Condition OK");
            return;
        }
        skill.LikeChannelling(null);
    }
}