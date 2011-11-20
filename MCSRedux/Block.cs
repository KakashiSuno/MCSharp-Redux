using System;
using System.Collections.Generic;
using System.Text;

namespace Minecraft_Server
{
	public class Block
	{
		public const byte air = (byte)0;
		public const byte rock = (byte)1;
		public const byte grass = (byte)2;
		public const byte dirt = (byte)3;
		public const byte stone = (byte)4;
		public const byte wood = (byte)5;
		public const byte shrub = (byte)6;
		public const byte blackrock = (byte)7;// adminium
		public const byte water = (byte)8;
		public const byte waterstill = (byte)9;
		public const byte lava = (byte)10;
		public const byte lavastill = (byte)11;
		public const byte sand = (byte)12;
		public const byte gravel = (byte)13;
		public const byte goldrock = (byte)14;
		public const byte ironrock = (byte)15;
		public const byte coal = (byte)16;
		public const byte trunk = (byte)17;
		public const byte leaf = (byte)18;
		public const byte sponge = (byte)19;
		public const byte glass = (byte)20;
		public const byte red = (byte)21;
		public const byte orange = (byte)22;
		public const byte yellow = (byte)23;
		public const byte lightgreen = (byte)24;
		public const byte green = (byte)25;
		public const byte aquagreen = (byte)26;
		public const byte cyan = (byte)27;
		public const byte lightblue = (byte)28;
		public const byte blue = (byte)29;
		public const byte purple = (byte)30;
		public const byte lightpurple = (byte)31;
		public const byte pink = (byte)32;
		public const byte darkpink = (byte)33;
		public const byte darkgrey = (byte)34;
		public const byte lightgrey = (byte)35;
		public const byte white = (byte)36;
		public const byte yellowflower = (byte)37;
		public const byte redflower = (byte)38;
		public const byte mushroom = (byte)39;
		public const byte redmushroom = (byte)40;
		public const byte goldsolid = (byte)41;
		public const byte iron = (byte)42;
		public const byte staircasefull = (byte)43;
		public const byte staircasestep = (byte)44;
		public const byte brick = (byte)45;
		public const byte tnt = (byte)46;
		public const byte bookcase = (byte)47;
		public const byte stonevine = (byte)48;
		public const byte obsidian = (byte)49;
		public const byte Zero = 0xff;

        //Custom blocks

        public const byte op_glass = (byte)100;
        public const byte opsidian = (byte)101;
        public const byte op_brick = (byte)102;
        public const byte op_stone = (byte)103;
        public const byte op_cobblestone = (byte)104;
        public const byte op_air = (byte)105;
        public const byte op_water = (byte)106;

        public const byte wood_float = (byte)110;
        public const byte door = (byte)111;
        public const byte lava_fast = (byte)112;
        public const byte door2 = (byte)113;
        public const byte door3 = (byte)114;

        public const byte air_flood = (byte)200;
        public const byte door_air = (byte)201;
        public const byte air_flood_layer = (byte)202;
        public const byte air_flood_down = (byte)203;
        public const byte air_flood_up = (byte)204;
        public const byte door2_air = (byte)205;
        public const byte door3_air = (byte)206;

		public static bool Placable(byte type)
		{
			switch (type)
			{
				case Block.air:
				case Block.grass:
				case Block.blackrock:
                case Block.water:
                case Block.waterstill:
                case Block.lava:
                case Block.lavastill:
					return false;
			}

            if (type > 49) { return false; }
			return true;
		}
        public static bool AdvPlacable(byte type)   //returns true if ADV Builder is allowed to use these unplacable blocks
        {
            switch (type)
            {
                case Block.air:
                case Block.grass:
                case Block.waterstill:
                case Block.lavastill:
                case Block.door:
                case Block.door2:
                case Block.door3:
                    return true;
            }
            return false;
        }

        public static bool LightPass(byte type)
        {
            switch (type)
            {
                case Block.air:
                case Block.glass:
                case Block.op_air:
                case Block.op_glass:
                case Block.leaf:
                case Block.redflower:
                case Block.yellowflower:
                case Block.mushroom:
                case Block.redmushroom:
                case Block.shrub:
                case Block.door3:
                    return true;

                default:
                    return false;
            }
        }

        public static bool Physics(byte type)   //returns false if placing block cant actualy cause any physics to happen
        {
            switch (type)
            {
                case Block.rock:
                case Block.stone:
                case Block.blackrock:
                case Block.waterstill:
                case Block.lavastill:
                case Block.goldrock:
                case Block.ironrock:
                case Block.coal:
                case Block.red:
                case Block.orange:
                case Block.yellow:
                case Block.lightgreen:
                case Block.green:
                case Block.aquagreen:
                case Block.cyan:
                case Block.lightblue:
                case Block.blue:
                case Block.purple:
                case Block.lightpurple:
                case Block.pink:
                case Block.darkpink:
                case Block.darkgrey:
                case Block.lightgrey:
                case Block.white:
                case Block.goldsolid:
                case Block.iron:
                case Block.staircasefull:
                case Block.brick:
                case Block.tnt:
                case Block.stonevine:
                case Block.obsidian:

                case Block.op_glass:
                case Block.opsidian:
                case Block.op_brick:
                case Block.op_stone:
                case Block.op_cobblestone:
                case Block.op_air:
                case Block.op_water:

                case Block.door:
                case Block.door2:
                case Block.door3:

                    return false;

                default:
                    return true;
            }
        }

		public static string Name(byte type)
        {
			switch(type)
            {
                case 0: return "air";  
                case 1: return "stone";
                case 2: return "grass"; 
                case 3: return "dirt";
                case 4: return "cobblestone";
                case 5: return "wood";
                case 6: return "plant"; 
                case 7: return "adminium"; 
                case 8: return "active_water";  
                case 9: return "water";  
                case 10: return "active_lava";  
                case 11: return "lava";  
                case 12: return "sand";
                case 13: return "gravel";
                case 14: return "gold_ore";
                case 15: return "iron_ore";
                case 16: return "coal";
                case 17: return "tree";
                case 18: return "leaves";
                case 19: return "sponge";
                case 20: return "glass";
                case 21: return "red";
                case 22: return "orange";
                case 23: return "yellow";
                case 24: return "greenyellow";
                case 25: return "green";
                case 26: return "springgreen";
                case 27: return "cyan";
                case 28: return "blue";
                case 29: return "blueviolet";
                case 30: return "indigo";
                case 31: return "purple";
                case 32: return "magenta";
                case 33: return "pink";
                case 34: return "black";
                case 35: return "gray";
                case 36: return "white";
                case 37: return "yellow_flower"; 
                case 38: return "red_flower"; 
                case 39: return "brown_shroom"; 
                case 40: return "red_shroom"; 
                case 41: return "gold";
                case 42: return "iron";
                case 43: return "double_stair"; 
                case 44: return "stair";
                case 45: return "brick";
                case 46: return "tnt";
                case 47: return "bookcase";
                case 48: return "mossy_cobblestone";
                case 49: return "obsidian";
                case 100: return "op_glass"; 
                case 101: return "opsidian";              //TODO Add command or just use bind?
                case 102: return "op_brick";              //TODO
                case 103: return "op_stone";              //TODO
                case 104: return "op_cobblestone";        //TODO
                case 105: return "op_air";                //TODO
                case 106: return "op_water";              //TODO

                case 110: return "wood_float";            //TODO
                case 111: return "door";
                case 112: return "lava_fast";
                case 113: return "door2";
                case 114: return "door3";

                //Blocks after this are converted before saving
                case 200: return "air_flood"; 
                case 201: return "door_air";
                case 202: return "air_flood_layer";
                case 203: return "air_flood_down";
                case 204: return "air_flood_up";
                case 205: return "door2_air";
                case 206: return "door3_air";
			    default: return "unknown";
            }
		}
        public static byte Byte(string type)
        {
            switch (type)
            {
                case "air": return 0;
                case "stone": return 1;
                case "grass": return 2;
                case "dirt": return 3;
                case "cobblestone": return 4;
                case "wood": return 5;
                case "plant": return 6;
                case "adminium": return 7;
                case "active_water": return 8;
                case "water": return 9;
                case "active_lava": return 10;
                case "lava": return 11;
                case "sand": return 12;
                case "gravel": return 13;
                case "gold_ore": return 14;
                case "iron_ore": return 15;
                case "coal": return 16;
                case "tree": return 17;
                case "leaves": return 18;
                case "sponge": return 19;
                case "glass": return 20;
                case "red": return 21;
                case "orange": return 22;
                case "yellow": return 23;
                case "greenyellow": return 24;
                case "green": return 25;
                case "springgreen": return 26;
                case "cyan": return 27;
                case "blue": return 28;
                case "blueviolet": return 29;
                case "indigo": return 30;
                case "purple": return 31;
                case "magenta": return 32;
                case "pink": return 33;
                case "black": return 34;
                case "gray": return 35;
                case "white": return 36;
                case "yellow_flower": return 37;
                case "red_flower": return 38;
                case "brown_shroom": return 39;
                case "red_shroom": return 40;
                case "gold": return 41;
                case "iron": return 42;
                case "double_stair": return 43;
                case "stair": return 44;
                case "brick": return 45;
                case "tnt": return 46;
                case "bookcase": return 47;
                case "mossy_cobblestone": return 48;
                case "obsidian": return 49;

                case "op_glass": return 100;
                case "opsidian": return 101;              //TODO Add command or just use bind?
                case "op_brick": return 102;              //TODO
                case "op_stone": return 103;              //TODO
                case "op_cobblestone": return 104;        //TODO
                case "op_air": return 105;                //TODO
                case "op_water": return 106;              //TODO

                case "wood_float": return 110;            //TODO
                case "door": return 111;
                case "lava_fast": return 112;
                case "door2": return 113;
                case "door3": return 114;

                //Blocks after this are converted before saving
                case "air_flood": return 200;
                case "door_air": return 201;
                case "air_flood_layer": return 202;
                case "air_flood_down": return 203;
                case "air_flood_up": return 204;
                case "door2_air": return 205;
                case "door3_air": return 206;
                default: return 255;
            }
        }

        public static byte Convert(byte b)
        {
            switch (b)
            {
                case 100: return (byte)20; //Op_glass
                case 101: return (byte)49; //Opsidian
                case 102: return (byte)45; //Op_brick
                case 103: return (byte)1; //Op_stone
                case 104: return (byte)4; //Op_cobblestone
                case 105: return (byte)0; //Op_air - Must be cuboided / replaced
                case 106: return (byte)9; //Op_water

                case 110: return (byte)5; //wood_float
                case 111: return (byte)17;//door show by treetype
                case 112: return (byte)10;

                case 113: return (byte)49;//door show by obsidian
                case 114: return (byte)20;//door show by glass

                case 200: //air_flood
                case 201: //door_air
                case 202: //air_flood_layer
                case 203: //air_flood_down
                case 204: //air_flood_up
                case 205: //door2_air
                case 206: //door3_air
                    return (byte)0; 
                default: return b;
            }
        }
        public static byte SaveConvert(byte b)
        {
            switch (b)
            {
                case 200:
                case 202:
                case 203:
                case 204: 
                    return (byte)0; //air_flood must be converted to air on save to prevent issues
                case 201: return (byte)111; //door_air back into door
                case 205: return (byte)113; //door_air back into door
                case 206: return (byte)114; //door_air back into door
                default: return b;
            }
        }
	}

	
	
}
