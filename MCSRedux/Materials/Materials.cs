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
        AIR = 0,
        STONE = 1,
        GRASS = 2,
        DIRT = 3,
        COBBLESTONE = 4,
        WOOD = 5,
        SAPLING = 6,
        BEDROCK = 7,
        WATER = 8,
        STATIONARY_WATER = 9,
        LAVA = 10,
        STATIONARY_LAVA = 11,
        SAND = 12,
        GRAVEL = 13,
        GOLD_ORE = 14,
        IRON_ORE = 15,
        COAL_ORE = 16,
        LOG = 17,
        LEAVES = 18,
        SPONGE = 19,
        GLASS = 20,
        LAPIS_ORE = 21,
        LAPIS_BLOCK = 22,
        DISPENSER = 23,
        SANDSTONE = 24,
        NOTE_BLOCK = 25,
        BED_BLOCK = 26,
        POWERED_RAIL = 27,
        DETECTOR_RAIL = 28,
        PISTON_STICKY_BASE = 29,
        WEB = 30,
        LONG_GRASS = 31,
        DEAD_BUSH = 32,
        PISTON_BASE = 33,
        PISTON_EXTENSION = 34,
        WOOL = 35,
        PISTON_MOVING_PIECE = 36,
        YELLOW_FLOWER = 37,
        RED_ROSE = 38,
        BROWN_MUSHROOM = 39,
        RED_MUSHROOM = 40,
        GOLD_BLOCK = 41,
        IRON_BLOCK = 42,
        DOUBLE_STEP = 43,
        STEP = 44,
        BRICK = 45,
        TNT = 46,
        BOOKSHELF = 47,
        MOSSY_COBBLESTONE = 48,
        OBSIDIAN = 49,
        TORCH = 50,
        FIRE = 51,
        MOB_SPAWNER = 52,
        WOOD_STAIRS = 53,
        CHEST = 54,
        REDSTONE_WIRE = 55,
        DIAMOND_ORE = 56,
        DIAMOND_BLOCK = 57,
        WORKBENCH = 58,
        CROPS = 59,
        SOIL = 60,
        FURNACE = 61,
        BURNING_FURNACE = 62,
        SIGN_POST = 63,
        WOODEN_DOOR = 64,
        LADDER = 65,
        RAILS = 66,
        COBBLESTONE_STAIRS = 67,
        WALL_SIGN = 68,
        LEVER = 69,
        STONE_PLATE = 70,
        IRON_DOOR_BLOCK = 71,
        WOOD_PLATE = 72,
        REDSTONE_ORE = 73,
        GLOWING_REDSTONE_ORE = 74,
        REDSTONE_TORCH_OFF = 75,
        REDSTONE_TORCH_ON = 76,
        STONE_BUTTON = 77,
        SNOW = 78,
        ICE = 79,
        SNOW_BLOCK = 80,
        CACTUS = 81,
        CLAY = 82,
        SUGAR_CANE_BLOCK = 83,
        JUKEBOX = 84,
        FENCE = 85,
        PUMPKIN = 86,
        NETHERRACK = 87,
        SOUL_SAND = 88,
        GLOWSTONE = 89,
        PORTAL = 90,
        JACK_O_LANTERN = 91,
        CAKE_BLOCK = 92,
        DIODE_BLOCK_OFF = 93,
        DIODE_BLOCK_ON = 94,
        LOCKED_CHEST = 95,
        TRAP_DOOR = 96,
        MONSTER_EGGS = 97, 
        SMOOTH_BRICK = 98, 
        HUGE_MUSHROOM_1 = 99,
        HUGE_MUSHROOM_2 = 100,
        IRON_FENCE = 101,
        THIN_GLASS = 102,
        MELON_BLOCK = 103,
        PUMPKIN_STEM = 104,
        MELON_STEM = 105,
        VINE = 106,
        FENCE_GATE = 107,
        BRICK_STAIRS = 108,
        SMOOTH_STAIRS = 109,
        MYCEL = 110,
        WATER_LILY = 111,
        NETHER_BRICK = 112,
        NETHER_FENCE = 113,
        NETHER_BRICK_STAIRS = 114,
        NETHER_WARTS = 115,
        ENCHANTMENT_TABLE = 116,
        BREWING_STAND = 117,
        CAULDRON = 118,
        ENDER_PORTAL = 119,
        ENTER_PORTAL_FRAME = 120,
        WHITESTONE = 121
    }
    public enum Items : short
    {

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
