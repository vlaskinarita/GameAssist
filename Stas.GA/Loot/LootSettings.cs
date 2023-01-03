
using Newtonsoft.Json;

namespace Stas.GA {
    public class LootSettings : iSett {
        [JsonProperty]
        public bool b_quest = true;
        [JsonProperty]
        public bool b_league = true;
        [JsonProperty]
        public bool b_portal = true;
        [JsonProperty]
        public bool b_wisdom = true;
        [JsonProperty]
        public bool b_transmut = true;
        [JsonProperty]
        public bool b_augment = true;
        [JsonProperty]
        public bool b_alter = true;
        [JsonProperty]
        public bool b_arm_scrap = true;
        [JsonProperty]
        public bool b_whetstone = true;
        [JsonProperty]
        public bool b_maps = true; 
        [JsonProperty]
        public bool b_currency;
        [JsonProperty]
        public bool b_Claw;
        [JsonProperty]
        public bool b_Dagger;
        [JsonProperty]
        public bool b_Wand;
        [JsonProperty]
        public bool b_Body_Armour;
        [JsonProperty]
        public bool b_Boots;
        [JsonProperty]
        public bool b_Gloves;
        [JsonProperty]
        public bool b_Helmet;
        [JsonProperty]
        public bool b_Amulet;
        [JsonProperty]
        public bool b_Belt;
        [JsonProperty]
        public bool b_Ring;
        [JsonProperty]
        public bool b_small_bow;
        [JsonProperty]
        public bool b_6s_big;
        [JsonProperty]
        public bool b_6s_small;
        [JsonProperty]
        public bool b_6l_any;
    }
    //"Claw", "Dagger", "Wand", "Body Armour", "Boots", "Gloves", "Helmet", "Amulet", "Belt", "Ring"
    /*
Portal Scroll
Scroll of Wisdom
Orb of Transmutation
Orb of Augmentation
Orb of Alteration
Armourer's Scrap
Blacksmith's Whetstone	
     */
}
