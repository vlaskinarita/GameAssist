using V2 = System.Numerics.Vector2;
namespace Stas.GA;

public class AncestralProtection : aTotem {
    public AncestralProtection(Keys _key, int grange = 15, int mana_cost = 8, int cast_time = 650)
        : base(_key, grange, mana_cost, cast_time, 1) {
        //buff_name = "melee_ancestor_totem_attack_speed";
        using_time = 1200; //55+ tiks
    }
}
public class DecoyTotem : aTotem {
    public DecoyTotem(Keys _key, int gdist = 60, int cost = 8, 
        int cast_time = 450, int max_count = 3)
        : base(_key, gdist, cost, cast_time, max_count) {
        SetSkill("DecoyTotem");
    }
} 
public class ShrapnelBallista : aTotem {
    public ShrapnelBallista(Keys _key, int gdist = 70, int cost = 5, 
        int cast_time = 1370, int max_count = 3)
         : base(_key, gdist, cost, cast_time, max_count) {
        SetSkill("ShrapnelBallista");
    }
}
public class siege_balista : aTotem {
    public siege_balista(Keys _key, int gdist = 20, int cost = 8, 
        int cast_time = 450, int max_count = 3)
         : base(_key, gdist, cost, cast_time, max_count) {
        SetSkill("SiegeBallista");
      
    }
}
public class glacial_cascade_totem : aTotem {
    public glacial_cascade_totem(Keys _key, int gdist = 60, int cost = 8, 
        int cast_time = 400, int max_count = 3)
        : base(_key, gdist, cost, cast_time, max_count) {
        SetSkill("GlacialCascade");
        totem_buff_name = "player_glacial_cascade";
    }
}
/// <summary>
/// generic damage totem
/// </summary>
public class DamageTotem : aTotem {
    public DamageTotem(Keys _key, string int_name, int gdist = 70, int cost = 8, int cast_time = 450, int max_count = 3)
         : base(_key, gdist, cost, cast_time, max_count) {
        SetSkill(int_name);
    }
    //here no name for serching totems
    public override int curr_count => base.curr_count;
}
/// <summary>
/// like: ancestral protector
/// </summary>
public class aTotem : aSkill {
    protected string totem_buff_name;
    public int max_count {get;}=3;
    public virtual int curr_count {
        get {
            if (skill != null) {
                var cc = 0;// current totems count close
                foreach (var o in skill.actor.DeployedObjects) {
                    switch (o.TypeId) {
                        case DeployedObjectType.Totem:
                            if (o.Entity?.gdist_to_me < grange)
                                cc += 1;
                            break;
                        default:
                            break; //debug here
                    }
                }
                return cc;
            }
            return max_count;
        }
    }
    protected void SetSkill(string _name) {
        name = _name;
        me.GetComp<Actor>(out var act);
        if (act == null)
            return;
        skill = act.actor_skills.FirstOrDefault(s => s.Name == name);
    }
    public aTotem(Keys _key, int gdist = 60, int cost = 8, 
        int cast_time = 450, int _max_count=3)
        : base(_key, gdist, cost, cast_time, 0) {
        max_count = _max_count;
    }
    bool TotemsBeStop {
        get {
            var leader_faar = false;
            if (ui.curr_role != Role.Master) {
                var l_gist = ui.leader.gdist_to_me;
                //if leader move out
                if (l_gist > ui.tasker.GetFallowDist) {
                    leader_faar = true;
                }
            }
            if (ui.b_alt || leader_faar || curr_count == max_count) //|| ui.mapper.danger == 0
                return true;
            return false;
        }
    }
    public override void Run(Action do_after=null) {
        if (TotemsBeStop) {
            StopCast();
            do_after?.Invoke();
            return;
        }
        if (!b_started && curr_count < max_count) {
            StartCast();
            return;
        }
    }
}
