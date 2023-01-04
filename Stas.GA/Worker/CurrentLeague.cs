namespace Stas.GA;

public class Default : aWorker {
    public Default() {
        //b_use_low_life = true;
        //max_danger = 10; //must stop and cleare if use faster navigation algoritm
        //main = new Frenzy(Keys.D1, 80, 15, 480);
        main = new Smite(Keys.D1, 30, 9, 670);
        jump = new JumpSkill(Keys.D2, 45, 7, 150, 2400);
        //mark = new AssasianMark(Keys.RButton, 80, 31, 250);
        rb = new MoltenShell(Keys.RButton, 0, 9);
        //rb = new FrostGlobe(Keys.RButton, 8, 28, 320, 5000);
        //rb = new FrostBomb(Keys.RButton, 80, 15, 250, 2500);
        //mb = new StoneGolem(Keys.MButton);
        totem = new AncestralProtection(Keys.D3, 15, 11, 560);
        //totem = new siege_balista(Keys.D3, 80, 29, 60, 4);
        //totem = new glacial_cascade_totem(Keys.D3, 60, 51, 400);
        //d4 = new OrbOfStorms(Keys.D4, 15, 15, 250, 500);
        //d5 = new TempestShield(Keys.D5);
        d6 = new Vitality(Keys.D6);
        d7 = new Herald_of_Thunder(Keys.D6);
        //d6 = new Purity_of_elements(Keys.D6);
        //d7 = new Haste(Keys.D7);
        d8 = new Herald_of_Ash(Keys.D8);
        //d8 = new curse_elemental_weakness(Keys.D8);
        //d9 = new curse_temporal_chains(Keys.D9);
        ////d0 = new ArcticArmour(Keys.RButton);
        //d0 = new curse_enfeeble(Keys.D0);
        CountSkills();
        //BuffFromHit = new SmiteBuff(Keys.RButton);
    }
}
public class CurseBot : aWorker {
    public CurseBot() {
        b_use_low_life = true;
        max_danger = 10; //must stop and cleare if use fater navigation algoritm
        //main = new Frenzy(Keys.D1, 80, 15, 480);
        main = new WinterOrb(Keys.D1, 70, 5, 190);
        jump = new JumpSkill(Keys.D2, 45, 13, 150, 2400);
        mark = new AssasianMark(Keys.RButton, 80, 31, 250);
        //rb = new FrostGlobe(Keys.RButton, 8, 28, 320, 5000);
        //rb = new FrostBomb(Keys.RButton, 80, 15, 250, 2500);
        mb = new StoneGolem(Keys.MButton);
        //totem = new siege_balista(Keys.D3, 80, 29, 60, 4);
        totem = new glacial_cascade_totem(Keys.D3, 60, 51, 400);
        d4 = new OrbOfStorms(Keys.D4, 15, 15, 250, 500);
        //d5 = new TempestShield(Keys.D5);
        d6 = new Purity_of_elements(Keys.D6);
        d7 = new Haste(Keys.D7);
        d8 = new curse_elemental_weakness(Keys.D8);
        d9 = new curse_temporal_chains(Keys.D9);
        //d0 = new ArcticArmour(Keys.RButton);
        d0 = new curse_enfeeble(Keys.D0);
        CountSkills();
        //BuffFromHit = new SmiteBuff(Keys.RButton);
    }
}

public class ManaGuard : aWorker {
    public ManaGuard() {
        b_use_low_life = true;
        main = new Smite(Keys.D1, 30, 6);
        jump = new JumpSkill(Keys.D2, 45, 11, 150, 2210);
        link = new SoulLink(Keys.RButton, 70, 27, 280, "KaloTotem");
        mb = new StoneGolem(Keys.MButton);
        totem = new glacial_cascade_totem(Keys.D3, 60, 51, 400);
        d4 = new DefianceBaner(Keys.D4);
        d5 = new FrostGlobe(Keys.D5, 8, 42, 380);
        d6 = new Vitality(Keys.D6);
        d7 = new Clarity(Keys.D7);
        d8 = new Precision(Keys.D8);
        d9 = new Discipline(Keys.D9);
        d0 = new Determination(Keys.D0);
        CountSkills();
        //BuffFromHit = new SmiteBuff(Keys.RButton);
    }
}

public class Balista : aWorker { 
    public Balista() {
        b_use_low_life = true;
        main = new Frenzy(Keys.D1, 80, 15, 480);
        //main = new EtherealKnives(Keys.D1, 75, 19, 460);
        jump = new JumpSkill(Keys.D2, 45, 15, 0, 2290);
        ////jump = new JumpSkill(Keys.D2, 45, 7, 150, 2400);
        //rb = new SoulLink(Keys.RButton);
        mb = new LightningGolem(Keys.MButton);
        totem = new siege_balista(Keys.D3, 80, 29, 60, 13);
        //d3 = new glacial_cascade_totem(Keys.D3, 60, 20, 290);
        //d4 = new ArcticArmour(Keys.D4);
        //d5 = new TempestShield(Keys.D5);
        d6 = new Grace(Keys.D6);
        d7 = new Herald_of_Ice(Keys.D7);
        //d8 = new Herald_of_Thunder(Keys.D8);
        //d9 = new Herald_of_Ash(Keys.D9);
        //d0 = new Herald_of_Purity(Keys.D0);
        CountSkills();
        //BuffFromHit = new SmiteBuff(Keys.RButton);
    }
}

public class AuraBot : aWorker {
    public AuraBot() {
        b_use_low_life = true;
        main = new WinterOrb(Keys.D1, 70, 5, 240);
        jump = new JumpSkill(Keys.D2, 45, 10, 150, 2290); //dash no supprt
        //rb = new SoulLink(Keys.RButton);
        mb = new StoneGolem(Keys.MButton);
        totem = new glacial_cascade_totem(Keys.D3, 60, 67, 360);
        d4 = new Purity_of_fire(Keys.D4);
        d5 = new Purity_of_ace(Keys.D5);
        d6 = new Hatred(Keys.D6);
        d7 = new Wrath(Keys.D7);
        d8 = new Zealotry(Keys.D8);
        d9 = new Anger(Keys.D9);
        d0 = new Purity_of_lightning(Keys.D0);
        CountSkills();
        //BuffFromHit = new SmiteBuff(Keys.RButton);
    }
}