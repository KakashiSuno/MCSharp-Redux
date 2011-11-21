//  
//  Materials.cs
//  
//  Author:
//       MCSR Team <day7tech@gmail.com>
// 
//  Copyright (c) 2011 MCSR Team
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;

namespace MCSRedux.Materials
{
    public enum Block : byte
    {
        Air = 0,
        Stone = 1,
        Grass = 2,
        Dirt = 3,
        Cobblestone = 4,
        Wood = 5,
        Sapling = 6,
        Bedrock = 7,
        Water = 8,
        WaterStationary = 9,
        Lava = 10,
        LavaStationary = 11,
        Sand = 12,
        Gravel = 13,
        GoldOre = 14,
        IronOre = 15,
        CoalOre = 16,
        Log = 17,
        Leaves = 18,
        Sponge = 19,
        Glass = 20,
        LapisOre = 21,
        LapisBlock = 22,
        Dispenser = 23,
        Sandstone = 24,
        NoteBlock = 25,
        BedBlock = 26,
        PoweredRail = 27,
        DetectorRail = 28,
        PistonStickyBase = 29,
        Web = 30,
        LongGrass = 31,
        DeadBush = 32,
        PistonBase = 33,
        PistonExtension = 34,
        Wool = 35,
        PistonMovingPiece = 36,
        YellowFlower = 37,
        RedRose = 38,
        BrownMushroom = 39,
        RedMushroom = 40,
        GoldBlock = 41,
        IronBlock = 42,
        DoubleStep = 43,
        Step = 44,
        Brick = 45,
        TNT = 46,
        Bookshelf = 47,
        MossyCobblestone = 48,
        Obsidian = 49,
        Torch = 50,
        Fire = 51,
        MobSpawner = 52,
        WoodStairs = 53,
        Chest = 54,
        RedstoneWire = 55,
        DiamondOre = 56,
        DiamondBlock = 57,
        Workbench = 58,
        Crops = 59,
        Soil = 60,
        Furnace = 61,
        FurnaceBurning = 62,
        SignPost = 63,
        WoodenDoor = 64,
        Ladder = 65,
        Rails = 66,
        CobblestoneStairs = 67,
        WallSign = 68,
        Level = 69,
        StonePLate = 70,
        IronDoorBlock = 71,
        WoodPlate = 72,
        RedstoneOre = 73,
        RedstoneOreGlowing = 74,
        RedstoneTorchOff = 75,
        RedstoneTorchOn = 76,
        StoneButton = 77,
        Snow = 78,
        Ice = 79,
        SnowBlock = 80,
        Cactus = 81,
        Clay = 82,
        SugarCaneBlock = 83,
        Jukebox = 84,
        Fence = 85,
        Pumpkin = 86,
        Netherrack = 87,
        SoulSand = 88,
        Glowstone = 89,
        Portal = 90,
        JackOLantern = 91,
        CakeBlock = 92,
        DiodeBlockOff = 93,
        DiodeBlockOn = 94,
        LockedChest = 95,
        TrapDoor = 96,
        MonsterEggs = 97, 
        SmoothBrick = 98, 
        HugeMushroom1 = 99,
        HugeMushroom2 = 100,
        IronFence = 101,
        ThinGlass = 102,
        MelonBlock = 103,
        PumpkinStem = 104,
        MelonStem = 105,
        Vine = 106,
        FenceGate = 107,
        BrickStairs = 108,
        SmoothStairs = 109,
        Mycelium = 110,
        WaterLily = 111,
        NetherBrick = 112,
        NetherFence = 113,
        NetherBrickStairs = 114,
        NetherWarts = 115,
        EnchantmentTable = 116,
        BrewingStand = 117,
        Cauldron = 118,
        EnderPortal = 119,
        EnderPortalFrame = 120,
        Whitestone = 121
    }
    public enum Items : short
    {
        IronSpade = 256,
        IronPickaxe = 257,
        IronAxe = 258,
        FlintAndSteel = 259,
        Apple = 260,
        Bow = 261,
        Arrow = 262,
        Coal = 263,
        Diamond = 264,
        IronIngot = 265,
        GoldIngot = 266,
        IronSword = 267,
        WoodSword = 268,
        WoodSpade = 269,
        WoodPickaxe = 270,
        WoodAxe = 271,
        StoneSword = 272,
        StoneSpace = 273,
        StonePickaxe = 274,
        StoneAxe = 275,
        DiamondSword = 276,
        DiamondSpace = 277,
        DiamondPickaxe = 278,
        DiamondAxe = 279,
        Stick = 280,
        Bowl = 281,
        MushroomSoup = 282,
        GoldSword = 283,
        GoldSpade = 284,
        GoldPickaxe = 285,
        GoldAxe = 286,
        String = 287,
        Feather = 288,
        Gunpowder = 289,
        WoodHoe = 290,
        StoneHoe = 291,
        IronHoe = 292,
        DiamondHoe = 293,
        GoldHoe = 294,
        Seeds = 295,
        Wheat = 296,
        Bread = 297,
        LeatherHelmet = 298,
        LeatherChestplate = 299,
        LeatherLeggings = 300,
        LeatherBoots = 301,
        ChainmailHelmet = 302,
        ChainmailChestplate = 303,
        ChainmailLeggings = 304,
        ChainmailBoots = 305,
        IronHelmet = 306,
        IronChestplate = 307,
        IronLeggings = 308,
        IronBoots = 309,
        DiamondHelmet = 310,
        DiamondChestplate = 311,
        DiamondLeggings = 312,
        DiamondBoots = 313,
        GoldHelmet = 314,
        GoldChestplate = 315,
        GoldLeggings = 316,
        GoldBoots = 317,
        Flint = 318,
        Pork = 319,
        GrilledPork = 320,
        Painting = 321,
        GoldenApple = 322,
        Sign = 323,
        WoodDoor = 324,
        Bucket = 325,
        WaterBucket = 326,
        LavaBucket = 327,
        Minecart = 328,
        Saddle = 329,
        IronDoor = 330,
        Redstone = 331,
        SnowBall = 332,
        Boat = 333,
        Leather = 334,
        MilkBucket = 335,
        ClayBrick = 336,
        ClayBall = 337,
        SugarCane = 338,
        Paper = 339,
        Book = 340,
        Slime = 341,
        StorageMinecart = 342,
        PoweredMinecart = 343,
        Egg = 344,
        Compass = 345,
        FishingRod = 346,
        Watch = 347,
        GlowstoneDust = 348,
        RawFish = 349,
        CookedFish = 350,
        InkSack = 351,
        Bone = 352,
        Sugar = 353,
        Cake = 354,
        Bed = 255,
        Diode = 356,
        Cookie = 357,
        Map = 358,
        Shears = 359,
        Melon = 360,
        PumpkinSeeds = 361,
        MelonSeeds = 362,
        BeefRaw = 363,
        BeefCooked = 364,
        ChickenRaw = 365,
        ChickenCooked = 366,
        RottenFlesh = 367,
        EnderPearl = 368,
        BlazeRod = 369,
        GhastTear = 370,
        GoldNugget = 371,
        NetherStalk = 372,
        Potion = 373,
        GlassBottle = 374,
        SpiderEye = 375,
        FermentedSpiderEye = 376,
        BlazePowder = 377,
        MagmaCream = 378,
        BrewingStandItem = 379,
        CauldronItem = 380,
        EyeOfEnder = 381,
        SpeckledMelon = 382
    }
    public enum Record : short
    {
        Thirteen = 2256, Cat, Blocks, Chirp, Far, Mall, Mellohi, Stal, Strad, Ward, Eleven
    }

    public static class Metadata
    {

        public enum Wood : byte
        {
            Normal, Redwood, Birch
        }
        public enum Liquid : byte
        {
            Full = 0, LavaMax = 3, WaterMax = 7, Falling = 8
        }
        public enum Wool : byte
        {
            White, Orange, Magenta, LightBlue, Yellow, LightGreen, Pink, Gray,
            LightGray, Cyan, Purple, Blue, Brown, DarkGreen, Red, Black
        }
        public enum Dyes : byte
        {
            InkSack, RoseRed, CactusGreen, CocoBeans, LapisLazuli, Purple, Cyan, LightGray,
            Gray, Pink, Lime, DandelionYellow, LightBlue, Magenta, Orange, BoneMeal
        }
        public enum Torch : byte
        {
            South = 1, North, East, West, Standing
        }
        public enum Tracks : byte
        {
            EastWest, NorthSouth, RiseSouth, RiseNorth, RiseEast, RiseWest,
            NECorner, SECorner, SWCorner, NWCorner
        }
        public enum Ladders : byte
        {
            East = 2, West, North, South
        }
        public enum Stairs : byte
        {
            South, North, West, East
        }
        public enum Lever : byte
        {
            SouthWall = 1, NorthWall, WestWall, EastWall, EWGround, NSGround,
            IsFlipped = 8
        }
        public enum Door : byte
        {
            Northeast, Southeast, Southwest, Northwest,
            IsOpen = 4, IsTopHalf = 8
        }
        public enum Button : byte
        {
            SouthWall = 1, NorthWall, WestWall, EastWall,
            IsPressed = 8
        }
        public enum Sign : byte
        {
            West = 0, North = 4, East = 8, South = 0xC,
            WallEast = 2, WallWest = 3, WallNorth = 4, WallSouth = 5
        }
        public enum Furnace : byte
        {
            East = 2, West, North, South
        }
        public enum Pumpkin : byte
        {
            East, South, West, North
        }
    }

}
