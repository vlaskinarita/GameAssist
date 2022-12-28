using System.Drawing;
using Size2F = SharpDX.Size2F;
using Size2 = SharpDX.Size2;

namespace Stas.GA {
    public static class SpriteHelper {
        public static readonly Size2F MapIconsSize = new Size2F(14, 18);
        //public static readonly Size2F MyMapIcons = new Size2F(7, 8);
        public static RectangleF GetUV(MapIconsIndex index) {
            return GetUV((int)index, MapIconsSize);
        }

        public static RectangleF GetUV(int index, Size2F size) {
            if (index % (int)size.Width == 0) {
                return new RectangleF((size.Width - 1) / size.Width, ((int)(index / size.Width) - 1) / size.Height, 1 / size.Width,
                    1 / size.Height);
            }

            return new RectangleF((index % size.Width - 1) / size.Width, index / (int)size.Width / size.Height, 1 / size.Width,
                1 / size.Height);
        }

        public static RectangleF GetUV(Size2 index, Size2F size) {
            return new RectangleF((index.Width - 1) / size.Width, (index.Height - 1) / size.Height, 1 / size.Width, 1 / size.Height);
        }

        public static RectangleF GetUV(int x, int y, float width, float height) {
            return new RectangleF((x - 1) / width, (y - 1) / height, 1 / width, 1 / height);
        }
    }
    public enum MapIconsIndex {
        WrongLocationBot = 1, //start row index 0
        PartyMember = 2,
        OtherPlayer = 3,
        NPC = 4,
        Portal = 5,
        QuestObject = 6,
        QuestItem = 7,
        Shrine = 8,
        StoryGlyph = 9,
        Waypoint = 10,
        RedFlag = 11,
        BlueFlag = 12,
        GreenFlag = 13,
        AbyssStart = 14,

        AbyssCrack = 15,    //start row index=1
        MissionTarget = 16,
        MissionAlly = 17,
        PartyLeader = 18,
        Entrance = 19,
        Labyrinth = 20,
        StoneCircle = 21,
        LabyrinthLever = 22,
        LabyrinthDoor = 23,
        VaalSideArea = 24,
        LabyrinthEnchant = 25,
        LabyrinthGoldKey = 26,
        LabyrinthSilverKey = 27,
        Essence = 28,

        EntrancePortal = 29,    //start row index = 2
        Breach = 30,
        _31 = 31,
        I_died = 32,
        BestiaryDeadMonster = 33,
        BestiaryBloodAlter = 34,
        BestiaryBlueMonster = 35,
        BestiaryYellowBeast = 36,
        BestiaryBoss = 37,
        IncursionArchitectUpgrade = 38,
        IncursionArchitectReplace = 39,
        IncursionDoorLocked = 40,
        IncursionDoorOpen = 41,
        IncursionPortal = 42,

        IncursionSacrifice = 43,    //start row index = 3
        IncursionGem = 44,
        DelveMapViewer = 45,
        Sulphite = 46,
        LootFilterLargeBlueCircle = 47,
        LootFilterMediumBlueCircle = 48,
        LootFilterSmallBlueCircle = 49,
        z50 = 50,
        z51 = 51,
        z52 = 52,
        LootFilterLargePurpleCircle = 53,
        LootFilterMediumPurpleCircle = 54,
        LootFilterSmallPurpleCircle = 55,
        LootFilterLargeRedCircle = 56,

        LootFilterMediumRedCircle = 57, //start row index = 4
        LootFilterSmallRedCircle = 58,
        LargeWhiteCircle = 59,
        MediumWhiteCircle = 60,
        SmallWhiteCircle = 61,
        LootFilterLargeYellowCircle = 62,
        LootFilterMediumYellowCircle = 63,
        LootFilterSmallYellowCircle = 64,
        z65 = 65,
        z66 = 66,
        z67 = 67,
        z68 = 68,
        z69 = 69,
        LootFilterSmallGreenDiamond = 70,

        z71 = 71,                       //start row index = 5
        z72 = 72,
        z73 = 73,
        Archnemesis = 74,
        z75 = 75,
        z76 = 76,
        LakeStampingDevice = 77,
        opened_chest_smal = 78,
        opened_chest_midle = 79,
        opened_chest_big = 80,
        Skull = 81,
        Effect = 82,
        TrapGeer = 83,
        Short_life = 84,

        HeistAgility = 85,              //start row index = 6
        Heist_86 = 86,
        Heist_87 = 87,
        Heist_88 = 88,
        HeistDemolition = 89,
        Heist_90 = 90,
        Heist_91 = 91,
        Heist_92 = 92,
        Heist_93 = 93,
        Heist_94 = 94,
        RewardHeist = 95,
        num_96 = 96,
        num_97 = 97,
        num_98 = 98,

        question_mark = 99,             //start row index = 7
        done = 100,
        mirror = 101,
        currency0 = 102,
        currency1 = 103,
        currency2 = 104,
        currency3 = 105,
        currency4 = 106,
        currency5 = 107,
        currency6 = 108,
        Shard = 109,
        TimelessKaruiSplinter = 110,
        TimelessTemplarSplinter = 111,
        TimelessMarakethSplinter = 112,

        ArmourersScrap = 113,            //start row index = 8
        BlacksmithsWhetstone = 114,
        PortalScroll = 115,
        ScrollofWisdom = 116,
        nu8_4 = 117,
        nu8_5 = 118,
        Vampiric = 119,
        FlameBlood = 120,
        Heralding_minions = 121,
        AuraFasterRuner = 122,
        Cavestalker = 123,
        ProximityShield = 124,
        Spirit = 125,
        OrbofAugmentation = 126,

        ClusterJewel = 127,                 //start row index = 9
        _128 = 128,
        Amulet = 129,
        Trinked = 130,
        AmuletAndRing = 131,
        Dynamite = 132,
        Resonator = 133,
        ManaFlask = 134,
        Map = 135,
        links_6 = 136,
        socets_6 = 137,
        unknow = 138,
        FrameArrow = 139,
        unk_140 = 140,

        DangerArea = 141,                     //start row index = 10
        HarvestCollectorBlue1 = 142,
        HarvestCollectorBlue2 = 143,
        HarvestCollectorBlue = 144,
        HarvestCollectorYellow = 145,

        Exped_elitemarker = 146,
        Exped_monstermarker = 147,
        Exped_chest = 148,
        Exped_chest3 = 149,
        Exped_remnant = 150,
        Exped_chest2 = 151,
        ExpeditionDetonator = 152,
        ExpeditionStash = 153,
        Exped_chest_multy = 154,

        DelveRobot = 155,       //start row index = 11              
        DynamiteWall = 156,
        DelveWaypoint = 157,
        YellowWaypoint = 158,
        HideoutSymbol = 159,
        BetrayalSymbolCatarina = 160,
        BetrayalSymbolDjinn = 161,
        CraftingUnlockObject = 162,
        CraftingBench = 163,
        MapDevice = 164,
        Synthesis1 = 165,
        Synthesis2 = 166,
        Synthesis3 = 167,
        HarvestPortal = 168,
        StashPlayer = 169, //startning last -5
        StashGuild = 170,
        BetrayalSymbolCart = 171,
        LegionInitiator = 172,
        LegionGeneric = 173,
        RewardAbyss = 174,
        RewardArmour = 175,
        RewardBestiary = 176,
        RewardBreach = 177,
        RewardCurrency = 178,
        RewardDivinationCards = 179,
        RewardEssences = 180,
        RewardFossils = 181,
        RewardFragments = 182,

        RewardGems = 183,//starting last row-4
        RewardNiceBox = 184,
        RewardHarbinger = 185,
        RewardLabyrinth = 186,
        RewardMaps = 187,
        RewardPerandus = 188,
        RewardProphecy = 189,
        RewardScarabs = 190,
        RewardTalisman = 191,
        Ring = 192,
        RewardUniques = 193,
        RewardWeapons = 194,
        RewardWeaponEnchants = 195,
        RewardJewels = 196,

        RewardBlight = 197, //starting last row-3
        BlightCore = 198, //
        BlightSpawner = 199,
        BlightBoss = 200, //BlightMonster
        BlightPathActive = 201,
        BlightPathInactive = 202,
        CurioDisplay = 203,
        HarvestHorticraftingStation = 204,
        Flare = 205,
        RitualRewards = 206,
        UltimatumAltar = 207,
        Red_door = 208,
        Green_door = 209,
        HeistEscapeRoute = 210,

        RewardChestMetamorph = 211, //starting last -2
        RewardChestDelirium = 212,
        RewardCorrupted = 213,
        bleed = 214,
        poison = 215,
        frost = 216,
        fire = 217,
        lightning = 218,
        tmp_219 = 219,
        HeistStockpile = 220,
        RewardGeneric = 221,
        Barrel = 222,
        BigChest = 223,
        Azurite = 224,

        Heist_Jewels = 225,//starting last row-1 //ring
        Heist_Armour = 226,
        Heist_Weapons = 227,
        Heist_228 = 228,
        Heist_QualityCurrency = 229,
        Heist_Essences = 230,
        Heist_231 = 231,
        Heist_Jewellery = 232,
        Heist_233 = 233,
        Heist_234 = 234,
        Heist_Prophecies = 235,
        Heist_DivinationCards = 236,
        Heist_StackedDecks = 237,
        Heist_Uniques = 238,

        Heist_239 = 239, //starting last row //delve?
        Heist_240 = 240,
        Heist_241 = 241,
        Heist_242 = 242,
        Heist_243 = 243,
        Heist_244 = 244,
        Heist_245 = 245,
        Heist_246 = 246,
        Heist_Currency = 247,
        Heist_248 = 248,
        Heist_Gems = 249,
        Heist_Maps = 250,
        Heist_251 = 251,
        Heist_252 = 252,
    }
}
