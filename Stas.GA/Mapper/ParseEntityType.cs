using System;
using System.Linq;
using ImGuiNET;
using V2 = System.Numerics.Vector2;
using V3 = System.Numerics.Vector3;

namespace Stas.GA;
public partial class Entity : RemoteObjectBase {
    void ParseEntityType() {
        switch (this.eType) {
            // There is no use case (yet) to re-evaluate the following entity types
            case eTypes.SelfPlayer:
                return;
            case eTypes.OtherPlayer:
            case eTypes.Blockage:
            case eTypes.Shrine:
                return;
        }
      
        if (!GetComp<Render>(out var _)) {
            this.eType = eTypes.Useless;
        }
        else if (GetComp<MinimapIcon>(out var MinimapIcon)) {
            eType = eTypes.MinimapIcon;
            if (this.Path.StartsWith("Metadata/Chests/LeaguesExpedition")) {
                this.eType = eTypes.ExpeditionChest;
            }
            else if (this.Path.StartsWith("Metadata/Chests/LeagueHeist")) {
                this.eType = eTypes.HeistChest;
            }
        }
        else if (Path.StartsWith("Metadata/MiscellaneousObjects/Expedition/", StringComparison.Ordinal)
                    || Path.Contains("Leagues/Expedition")) {
            eType = eTypes.Exped;
        }
        else if (GetComp<AreaTransition>(out var are_trns) || Path.EndsWith("Portal")) {
            eType = eTypes.Portal;
        }
        else if ((GetComp<Targetable>(out var target) && GetComp<TriggerableBlockage>(out var _trigger))
                                     || Path.Contains("LabyrinthIzaroDoor")) {
            eType = eTypes.Door;
        }
        else if (Path.Contains("Quest") && !Path.Contains("MaligaroOrrery")
            & IsTargetable) {
            eType = eTypes.Quest;
        }
        else if (GetComp<Chest>(out var chestComp)) {
            if (chestComp.IsOpened) {
                this.eType = eTypes.Useless;
                var old = ui.curr_map.static_items.FirstOrDefault(mi => mi.Value.ent.id == id);
                if(old.Key >0)
                    ui.curr_map.static_items.TryRemove(old.Value.key, out _);
            }
            else if (this.eType == eTypes.Unidentified) { // so it only happen once.
                if (this.Path.StartsWith("Metadata/Chests/LegionChests")) {
                    this.eType = eTypes.Useless;
                }
                else if (this.Path.StartsWith("Metadata/Chests/DelveChests/")) {
                    this.eType = eTypes.DelveChest;
                }
                else if (this.Path.StartsWith("Metadata/Chests/Breach")) {
                    this.eType = eTypes.BreachChest;
                }
                else if (chestComp.IsStrongbox) {
                    if (this.Path.StartsWith("Metadata/Chests/StrongBoxes/Arcanist") ||
                        this.Path.StartsWith("Metadata/Chests/StrongBoxes/Cartographer") ||
                        this.Path.StartsWith("Metadata/Chests/StrongBoxes/StrongboxDivination") ||
                        this.Path.StartsWith("Metadata/Chests/StrongBoxes/StrongboxScarab")) {
                        this.eType = eTypes.ImportantStrongboxChest;
                    }
                    else {
                        this.eType = eTypes.StrongboxChest;
                    }
                }
                else if (chestComp.IsLarge) {
                    this.eType = eTypes.LargeChest;
                }
                else if (chestComp.OpenOnDamage) {
                    this.eType = eTypes.Barrel;
                }
                else {
                    this.eType = eTypes.Chest;
                }
            }
        }
        else if (GetComp<Player>(out var _)) {
            if (this.id == ui.states.ingame_state.area_instance.player.id) {
                this.eType = eTypes.SelfPlayer;
            }
            else {
                this.eType = eTypes.OtherPlayer;
            }
        }
        else if (GetComp<Shrine>(out var _)) {
            // NOTE: Do not send Shrine to useless because it can go back to not used.
            //       e.g. Shrine in PVP area can do that.
            this.eType = eTypes.Shrine;
        }
        else if (GetComp<Life>(out var lifeComp)) {
            if (!lifeComp.IsAlive) {
                this.eType = eTypes.Useless;
                return;
            }

            if (this.GetComp<TriggerableBlockage>(out var _)) {
                // NOTE: Do not send blockage to useless because it can go back to blocked.
                // NOTE: If blockage Life is 0 (not IsAlive), it can be send to useless
                //       but they are so rare, it's not worth it.
                this.eType = eTypes.Blockage;
                return;
            }

            if (!this.GetComp<Positioned>(out var posComp)) {
                this.eType = eTypes.Useless;
                return;
            }
            if (GetComp<PetAi>(out var pet)) {
                this.eType = eTypes.Pet;
                return;
            }
            if (!this.GetComp<ObjectMagicProperties>(out var OMP)) { //like pet
                this.eType = eTypes.Useless;
                return;
            }

            if (posComp.IsFriendly) {
                this.eType = eTypes.Friendly;
                return;
            }

            if (this.eType == eTypes.Unidentified && this.GetComp<DiesAfterTime>(out var _) &&
                   diesAfterTimeIgnore.Any(ignorePath => this.Path.StartsWith(ignorePath))) {
                this.eType = eTypes.Useless;
                return;
            }

            if (this.GetComp<Buffs>(out var buffComp)) {
                // When Legion monolith is not clicked by the user (Stage 0),
                //     Legion monsters (a.k.a FIT) has Frozen in time + Hidden buff.

                // When Legion monolith is clicked (Stage 1),
                //     FIT Not Killed by User: Just have frozen in time buff.
                //     FIT Killed by user: Just have hidden buff.

                // When Legion monolith is destroyed (Stage 2),
                //     FIT are basically same as regular monster with no Frozen-in-time/hidden buff.

                // NOTE: There are other hidden monsters in the game as well
                // e.g. Delirium monsters (a.k.a DELI), underground crabs, hidden sea witches
                var isFrozenInTime = buffComp.StatusEffects.ContainsKey("frozen_in_time");
                var isHidden = buffComp.StatusEffects.ContainsKey("hidden_monster");
                if (isFrozenInTime && isHidden) {
                    if (this.eType != eTypes.Stage0RewardFIT &&
                        this.eType != eTypes.Stage0EChestFIT &&
                        this.eType != eTypes.Stage0FIT) // New FITs only.
                    {
                        if (buffComp.StatusEffects.ContainsKey("legion_reward_display")) {
                            this.eType = eTypes.Stage0RewardFIT;
                        }
                        else if (this.Path.Contains("ChestEpic")) {
                            this.eType = eTypes.Stage0EChestFIT;
                        }
                        else if (this.Path.Contains("Chest")) {
                            this.eType = eTypes.Stage0RewardFIT;
                        }
                        else {
                            this.eType = eTypes.Stage0FIT;
                        }
                    }

                    return;
                }
                else if (isFrozenInTime) {
                    if (this.eType != eTypes.Stage1RewardFIT &&
                        this.eType != eTypes.Stage1EChestFIT &&
                        this.eType != eTypes.Stage1FIT) // New FITs only.
                    {
                        if (buffComp.StatusEffects.ContainsKey("legion_reward_display")) {
                            this.eType = eTypes.Stage1RewardFIT;
                        }
                        else if (this.Path.Contains("ChestEpic")) {
                            this.eType = eTypes.Stage1EChestFIT;
                        }
                        else if (this.Path.Contains("Chest")) {
                            this.eType = eTypes.Stage1RewardFIT;
                        }
                        else {
                            this.eType = eTypes.Stage1FIT;
                        }
                    }

                    return;
                }
                else if (isHidden) {
                    switch (this.eType) {
                        case eTypes.Stage0EChestFIT:
                        case eTypes.Stage0RewardFIT:
                        case eTypes.Stage0FIT:
                        case eTypes.Stage1EChestFIT:
                        case eTypes.Stage1RewardFIT:
                        case eTypes.Stage1FIT:
                        case eTypes.Stage1DeadFIT:
                            this.eType = eTypes.Stage1DeadFIT;
                            return;
                        case eTypes.DeliriumBomb:
                        case eTypes.DeliriumSpawner:
                            return;
                        case eTypes.Unidentified:
                            if (this.Path.StartsWith(deliriumHiddenMonsterStarting)) {
                                if (this.Path.Contains("BloodBag")) {
                                    this.eType = eTypes.DeliriumBomb;
                                }
                                else if (this.Path.Contains("EggFodder")) {
                                    this.eType = eTypes.DeliriumSpawner;
                                }
                                else if (this.Path.Contains("GlobSpawn")) {
                                    this.eType = eTypes.DeliriumSpawner;
                                }
                                else {
                                    this.eType = eTypes.Useless;
                                }

                                return;
                            }

                            break;
                    }
                }
            }
            this.eType = eTypes.Monster;
        }
        else if (GetComp<NPC>(out var npc)) {
            eType = eTypes.NPC;
        }
        else if (Path.Contains("MiscellaneousObjects/Door")) {
            eType = eTypes.Door;
        }
        else if (GetComp<WorldItem>(out var worl_item)) {
            if (!GetComp<Targetable>(out var trg)) {
            }
            else
                this.eType = eTypes.WorldItem;
        }
        else if (GetComp<Projectile>(out var Projectile) || Path.Contains("Projectiles")) {
            eType = eTypes.Projectile;
        }
        else if (GetComp<LimitedLifespan>(out var effect)) {//explosion with time
            if(ui.sett.b_develop)
                this.eType = eTypes.LimitedLife;
            else
                this.eType = eTypes.Useless;
        }
        else if (Path.Contains("Effects")) {
            if (ui.sett.b_develop)
                this.eType = eTypes.Effects;
            else
                this.eType = eTypes.Useless;
        }
        else if (Path.Contains("Waypoint")) {
            eType = eTypes.Waypoint;
        }
        else if (Path.Contains("AreaTransitionToggleable")) {
            eType = eTypes.Portal;
        }
        else if (Path.Contains("Terrain")) { //WorldDescription component mb
            eType = eTypes.Terrain;
        }
        else if (Path.Contains("MiscellaneousObjects")) {
            if(ui.sett.b_develop && ui.sett.b_draw_misk)
                eType = eTypes.Misc;
            else
                eType = eTypes.Useless;
        }
        else {
            if (Path.Contains("Doodad"))
                this.eType = eTypes.Useless;
            else {
                this.eType = eTypes.NeedCheck;
            }
        }
    }

}