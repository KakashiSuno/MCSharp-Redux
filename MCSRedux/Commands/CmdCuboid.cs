using System;
using System.Collections.Generic;

namespace Minecraft_Server
{
    public class CmdCuboid : Command
    {
        public override string name { get { return "cuboid"; } }
        public CmdCuboid() { }
        public override void Use(Player p, string message)
        {
            int number = message.Split(' ').Length;
            if (number > 2) { Help(p); return; }
            if (number == 2)
            {
                int pos = message.IndexOf(' ');
                string t = message.Substring(0, pos).ToLower();
                string s = message.Substring(pos + 1).ToLower();
                byte type = Block.Byte(t);
                if (type == 255) { p.SendMessage("There is no block \"" + t + "\"."); return; }
                if (Server.advbuilders.Contains(p.name))
                {
                    if (!Block.Placable(type) && !Block.AdvPlacable(type)) { p.SendMessage("Your not allowed to place that."); return; }
                }
                SolidType solid;
                if (s == "solid") { solid = SolidType.solid; }
                else if (s == "hollow") { solid = SolidType.hollow; }
                else if (s == "walls") { solid = SolidType.walls; }
                else { Help(p); return; }
                CatchPos cpos; cpos.solid = solid; cpos.type = type;
                cpos.x = 0; cpos.y = 0; cpos.z = 0; p.blockchangeObject = cpos;
            }
            else if (message != "")
            {
                SolidType solid = SolidType.solid;
                message = message.ToLower();
                byte type; unchecked { type = (byte)-1; }
                if (message == "solid") { solid = SolidType.solid; }
                else if (message == "hollow") { solid = SolidType.hollow; }
                else if (message == "walls") { solid = SolidType.walls; }
                else
                {
                    byte t = Block.Byte(message);
                    if (t == 255) { p.SendMessage("There is no block \"" + message + "\"."); return; }
                    if (Server.advbuilders.Contains(p.name))
                    {
                        if (!Block.Placable(t) && !Block.AdvPlacable(t)) { p.SendMessage("Your not allowed to place that."); return; }
                    }
                    type = t;
                } CatchPos cpos; cpos.solid = solid; cpos.type = type;
                cpos.x = 0; cpos.y = 0; cpos.z = 0; p.blockchangeObject = cpos;
            }
            else
            {
                CatchPos cpos; cpos.solid = SolidType.solid; unchecked { cpos.type = (byte)-1; }
                cpos.x = 0; cpos.y = 0; cpos.z = 0; p.blockchangeObject = cpos;
            }
            p.SendMessage("Place two blocks to determine the edges.");
            p.ClearBlockchange();
            p.Blockchange += new Player.BlockchangeEventHandler(Blockchange1);
        }
        public override void Help(Player p)
        {
            p.SendMessage("/cuboid [type] <solid/hollow/walls> - create a cuboid of blocks.");
        }
        public void Blockchange1(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();
            byte b = p.level.GetTile(x, y, z);
            p.SendBlockchange(x, y, z, b);
            CatchPos bp = (CatchPos)p.blockchangeObject;
            bp.x = x; bp.y = y; bp.z = z; p.blockchangeObject = bp;
            p.Blockchange += new Player.BlockchangeEventHandler(Blockchange2);
        }
        public void Blockchange2(Player p, ushort x, ushort y, ushort z, byte type)
        {
            p.ClearBlockchange();
            byte b = p.level.GetTile(x, y, z);
            p.SendBlockchange(x, y, z, b);
            CatchPos cpos = (CatchPos)p.blockchangeObject;
            unchecked { if (cpos.type != (byte)-1) { type = cpos.type; } }
            List<Pos> buffer = new List<Pos>();
            switch (cpos.solid)
            {
                case SolidType.solid:
                    if (!Server.superOps.Contains(p.name))
                    {
                        if (Math.Abs(cpos.x - x) * Math.Abs(cpos.y - y) * Math.Abs(cpos.z - z) > 1200)
                        {
                            p.SendMessage("Too many blocks, build in stages.");
                            return;
                        }
                    }
                    buffer.Capacity = Math.Abs(cpos.x - x) * Math.Abs(cpos.y - y) * Math.Abs(cpos.z - z);
                    for (ushort xx = Math.Min(cpos.x, x); xx <= Math.Max(cpos.x, x); ++xx)
                        for (ushort yy = Math.Min(cpos.y, y); yy <= Math.Max(cpos.y, y); ++yy)
                            for (ushort zz = Math.Min(cpos.z, z); zz <= Math.Max(cpos.z, z); ++zz)
                                if (p.level.GetTile(xx, yy, zz) != type){BufferAdd(buffer, xx, yy, zz);}
                    break;
                case SolidType.hollow:
                    //todo work out if theres 800 blocks used before making the buffer
                    for (ushort yy = Math.Min(cpos.y, y); yy <= Math.Max(cpos.y, y); ++yy)
                        for (ushort zz = Math.Min(cpos.z, z); zz <= Math.Max(cpos.z, z); ++zz)
                        {
                            if (p.level.GetTile(cpos.x, yy, zz) != type) { BufferAdd(buffer, cpos.x, yy, zz); }
                            if (cpos.x != x) { if (p.level.GetTile(x, yy, zz) != type) { BufferAdd(buffer, x, yy, zz); } }
                        }
                    if (Math.Abs(cpos.x - x) >= 2)
                    {
                        for (ushort xx = (ushort)(Math.Min(cpos.x, x) + 1); xx <= Math.Max(cpos.x, x) - 1; ++xx)
                            for (ushort zz = Math.Min(cpos.z, z); zz <= Math.Max(cpos.z, z); ++zz)
                            {
                                if (p.level.GetTile(xx, cpos.y, zz) != type) { BufferAdd(buffer, xx, cpos.y, zz); }
                                if (cpos.y != y) { if (p.level.GetTile(xx, y, zz) != type) { BufferAdd(buffer, xx, y, zz); } }
                            }
                        if (Math.Abs(cpos.y - y) >= 2)
                        {
                            for (ushort xx = (ushort)(Math.Min(cpos.x, x) + 1); xx <= Math.Max(cpos.x, x) - 1; ++xx)
                                for (ushort yy = (ushort)(Math.Min(cpos.y, y) + 1); yy <= Math.Max(cpos.y, y) - 1; ++yy)
                                {
                                    if (p.level.GetTile(xx, yy, cpos.z) != type) { BufferAdd(buffer, xx, yy, cpos.z); }
                                    if (cpos.z != z) { if (p.level.GetTile(xx, yy, z) != type) { BufferAdd(buffer, xx, yy, z); } }
                                }
                        }
                    }
                    break;
                case SolidType.walls:
                    for (ushort yy = Math.Min(cpos.y, y); yy <= Math.Max(cpos.y, y); ++yy)
                        for (ushort zz = Math.Min(cpos.z, z); zz <= Math.Max(cpos.z, z); ++zz)
                        {
                            if (p.level.GetTile(cpos.x, yy, zz) != type) { BufferAdd(buffer, cpos.x, yy, zz); }
                            if (cpos.x != x) { if (p.level.GetTile(x, yy, zz) != type) { BufferAdd(buffer, x, yy, zz); } }
                        }
                    if (Math.Abs(cpos.x - x) >= 2)
                    {
                        if (Math.Abs(cpos.z - z) >= 2)
                        {
                            for (ushort xx = (ushort)(Math.Min(cpos.x, x) + 1); xx <= Math.Max(cpos.x, x) - 1; ++xx)
                                for (ushort yy = (ushort)(Math.Min(cpos.y, y)); yy <= Math.Max(cpos.y, y); ++yy)
                                {
                                    if (p.level.GetTile(xx, yy, cpos.z) != type) { BufferAdd(buffer, xx, yy, cpos.z); }
                                    if (cpos.z != z) { if (p.level.GetTile(xx, yy, z) != type) { BufferAdd(buffer, xx, yy, z); } }
                                }
                        }
                    }
                    break;
            }

            if (!Server.superOps.Contains(p.name))
            {
                if (buffer.Count > 1200)
                {
                    p.SendMessage("Too many blocks, build in stages.");
                    return;
                }
            }
            else if (buffer.Count > 50000)
            {
                p.SendMessage("That is a bad idea.");
                return;
            }

            p.SendMessage(buffer.Count.ToString() + " blocks.");
            if (!Server.advbuilders.Contains(p.name))
            {
                buffer.ForEach(delegate(Pos pos)
                {
                    p.level.Blockchange(p, pos.x, pos.y, pos.z, type);                  //update block for everyone
                });
            }
            else
            {
                buffer.ForEach(delegate(Pos pos)
                {
                    byte bl = p.level.GetTile(pos.x, pos.y, pos.z);
                    if (Block.Placable(bl) || Block.AdvPlacable(bl)) { p.level.Blockchange(p, pos.x, pos.y, pos.z, type); }                  //update block for everyone
                });
            }

            
        }
        void BufferAdd(List<Pos> list, ushort x, ushort y, ushort z)
        {
            Pos pos; pos.x = x; pos.y = y; pos.z = z; list.Add(pos);
        }
        struct Pos
        {
            public ushort x, y, z;
        }
        struct CatchPos
        {
            public SolidType solid;
            public byte type;
            public ushort x, y, z;
        }
        enum SolidType { solid, hollow, walls };
    }
}