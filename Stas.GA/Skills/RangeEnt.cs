using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using V2 = System.Numerics.Vector2;

namespace Stas.GA {
 public class RangeAttack : aSkill {
        public RangeAttack(Keys _key, int gdist = 70, int cost = 8, int cast_time = 750)
            : base(_key, gdist, cost, cast_time, 0) {
            b_need_target = true;
        }
        public override void Run(Action do_after) {
            this.LikeChannelling(do_after);
        }
    }
    public class ToxicRain : aSkill {
        public ToxicRain(Keys _key, int gdist = 70, int cost = 5, int cast_time = 900, int cooldown = 0)
             : base(_key, gdist, cost, cast_time, cooldown) {
            using_time = 1000;
        }
        public override void Run(Action do_after) {
            if (!b_started) {
                StartCast();
                last_use = DateTime.Now;
            } else {
                if (ui.curr_map.danger == 0 || last_use.AddMilliseconds(using_time) < DateTime.Now) {
                    StopCast();
                    do_after?.Invoke();
                }
            }
        }
    }
    public class NoobSpell : aSkill {
        public NoobSpell(Keys _key, int gdist=70,  int cost=5, int cast_time=900, int cooldown=0) 
            : base(_key, gdist, cost, cast_time, cooldown) {
            using_time = 1000;
            b_can_pull = true;
        }
        public override void Run(Action do_after) {
            if (!b_started) {
                StartCast();
                last_use = DateTime.Now;
            } else {
                if (MustBeStop() || last_use.AddMilliseconds(using_time) < DateTime.Now) {
                    StopCast();
                    do_after?.Invoke();
                }
            }
        }
    }
    public class ArcanistBrande : aOneHitSkill { 
        public ArcanistBrande(Keys _key, int gdist = 80, int cost = 17, int cast_time = 470, int cooldown = 0)
            : base(_key, gdist, cost, cast_time, cooldown) {
            b_can_pull = true;
        }
    }
    public class StormBrand : aSkill {
        public StormBrand(Keys _key, int gdist = 70, int cost = 16, int cast_time = 640, int cooldown = 0)
            : base(_key, gdist, cost, cast_time, cooldown) { 
        }
        public override void Run(Action do_after) {
            LikeChannelling(do_after);
        }
    }
    public class PenanceBrande : aSkill { 
        public PenanceBrande(Keys _key) : base(_key, 70, 56, 380, 0) {
        }
        public override void Run(Action do_after) {
            LikeChannelling(do_after);
        }
    }
    //public class KineticBlast :aEntSkill {//"kinetic_blast"
    //    public KineticBlast(Keys _key) : base(_key, ) {
    //        grange = 47;//45;
    //    }
    //    public override void Run() {
    //        LikeChannelling();
    //    }
    //}
    public class ForbiddenRite : aSkill {
        public ForbiddenRite(Keys _key) : base(_key, 70, 28, 580, 0) {
        }
        public override void Run(Action do_after) {
            LikeChannelling(do_after);
        }
    } 

   
    public class Spark : aSkill { //
        public Spark(Keys _key) : base(_key, 80, 40, 150, 0) {

        }
        public override void Run(Action do_after) {
            this.LikeChannelling(do_after);
        }
    }
    public class KineticBlast : aSkill { //
        public KineticBlast(Keys _key) : base(_key, 80, 73, 320, 0) {

        }
        public override void Run(Action do_after) {
            this.LikeChannelling(do_after);
        }
    }
    public class Exsanguinate : aSkill {//"player_glacial_cascade"
        public Exsanguinate(Keys _key) : base(_key, 60, 1, 440, 0) {
        }

        public override void Run(Action do_after) {
            this.LikeChannelling(do_after);
        }
    }
   public class Frenzy : aSkill { //
        public Frenzy(Keys _key, int gdist = 75, int cost = 15, int cast_time = 480, int cooldown = 0)
            : base(_key, gdist, cost, cast_time, cooldown) {
            b_can_pull = true;
        }
        public override void Run(Action do_after) {
            this.LikeChannelling(do_after);
        }
    }
    public class WaveOfConviction : aSkill { //name=Purge in=divine_tempest
        public WaveOfConviction(Keys _key, int gdist = 60, int cost = 15, int cast_time = 480, int cooldown = 0)
            : base(_key, gdist, cost, cast_time, cooldown) {
            b_can_pull = true;
        }
        public override void Run(Action do_after) {
            this.LikeChannelling(do_after);
        }
    }
    public class EtherealKnives : aSkill { //
        public EtherealKnives(Keys _key, int gdist = 75, int cost = 5, int cast_time = 600, int cooldown = 0)
            : base(_key, gdist, cost, cast_time, cooldown) {
            b_can_pull = true;
        }
        public override void Run(Action do_after) {
            this.LikeChannelling(do_after);
        }
    }
    public class aRangeSpell : aSkill {
        public aRangeSpell(Keys _key, int gdist = 75, int cost = 5, int cast_time = 600, int cooldown = 0)
            : base(_key, gdist, cost, cast_time, cooldown) {
        }

        public override void Run(Action do_after) {
            LikeChannelling(do_after);
        }
    }
    public class GlacialCcascade : aSkill {
        public GlacialCcascade(Keys _key, int gdist = 75, int cost = 5, int cast_time = 600, int cooldown = 0)
            : base(_key, gdist, cost, cast_time, cooldown) { 
        }

        public override void Run(Action do_after) {
            LikeChannelling(do_after);
        }
    }
    //class BallLightnin :aRangeAOE {
    //    public BallLightnin(Keys _key) : base(_key) {
    //        grange = 70;
    //        intern_name = "ball_lightning";
    //    }

    //    public override void Set(V2 tgp) {
    //        LikeChannelling();
    //    }
    //}
    //public class FreezingPulse :aEntSkill {
    //    public FreezingPulse(Keys _key) : base(_key, "freezing_pulse") {
    //        grange = 70;
    //    }
    //    public override void Run() {
    //        LikeChannelling();
    //    }
    //}

    //class Frostbolt :aRangeAOE {
    //    public Frostbolt(Keys _key) : base(_key) {
    //        grange = 70;
    //    }
    //    public override void Set(V2 tgp) {
    //        LikeChannelling();
    //    }
    //}
}
