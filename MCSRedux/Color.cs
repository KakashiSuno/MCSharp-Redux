using System;

namespace Minecraft_Server {
	public static class c {
		public const string black = "&0";
		public const string navy = "&1";
		public const string green = "&2";
		public const string teal = "&3";
		public const string maroon = "&4";
		public const string purple = "&5";
		public const string gold = "&6";
		public const string silver = "&7";
		public const string gray = "&8";
		public const string blue = "&9";
		public const string lime = "&a";
		public const string aqua = "&b";
		public const string red = "&c";
		public const string pink = "&d";
		public const string yellow = "&e";
		public const string white = "&f";

		public static string Parse(string str) {
			switch (str.ToLower()) {
					case "black": return black;
					case "navy": return navy;
					case "green": return green;
					case "teal": return teal;
					case "maroon": return maroon;
					case "purple": return purple;
					case "gold": return gold;
					case "silver": return silver;
					case "gray": return gray;
					case "blue": return blue;
					case "lime": return lime;
					case "aqua": return aqua;
					case "red": return red;
					case "pink": return pink;
					case "yellow": return yellow;
					case "white": return white;
					default: return "";
			}
		} public static string Name(string str) {
			switch (str) {
					case black: return "black";
					case navy: return "navy";
					case green: return "green";
					case teal: return "teal";
					case maroon: return "maroon";
					case purple: return "purple";
					case gold: return "gold";
					case silver: return "silver";
					case gray: return "gray";
					case blue: return "blue";
					case lime: return "lime";
					case aqua: return "aqua";
					case red: return "red";
					case pink: return "pink";
					case yellow: return "yellow";
					case white: return "white";
					default: return "";
			}
		}
	}
}