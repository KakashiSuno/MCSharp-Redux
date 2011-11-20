using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

///WARNING! DO NOT CHANGE THE WAY THE LEVEL IS SAVED/LOADED!
///You MUST make it able to save and load as a new version other wise you will make old levels incompatible!


namespace Minecraft_Server
{
    public enum LevelPermission
    {
        Null = 0x99,
        Guest = 0x00,
        Builder = 0x01,
        AdvBuilder = 0x02,
        Moderator = 0x03,
        Operator = 0x04,
        Admin = 0x05
	}

	public class Level
    {
        public string name;
        public ushort width; // x
        public ushort depth; // y       THIS IS STUPID, SHOULD HAVE BEEN Z
        public ushort height; // z      THIS IS STUPID, SHOULD HAVE BEEN Y

        public ushort spawnx;
        public ushort spawny;
        public ushort spawnz;
        public byte rotx;
        public byte roty;

        public int physics = 0;
        public LevelPermission permissionvisit = LevelPermission.Builder;
        public LevelPermission permissionbuild = LevelPermission.Builder;// What ranks can go to this map (excludes banned)

        public byte[] blocks;
        List<Check> ListCheck = new List<Check>();  //A list of blocks that need to be updated
        List<Update> ListUpdate = new List<Update>();  //A list of block to change after calculation

        public bool changed = false;
        //public List<Edit> edits = new List<Edit>(1024);
        public Level(string n, ushort x, ushort y, ushort z, string type)
        {
            width = x; depth = y; height = z;

            if (width < 16) { width = 16; }
            if (depth < 16) { depth = 16; }
            if (height < 16) { height = 16; }

            name = n; blocks = new byte[width * depth * height];
            

            switch (type)
            {
                case "flat":
                case "pixel":
                    ushort half = (ushort)(depth / 2);
                    for (x = 0; x < width; ++x)
                    {
                        for (z = 0; z < height; ++z)
                        {
                            for (y = 0; y < depth; ++y)
                            {
                                //Block b = new Block();
                                switch (type)
                                {
                                    case "flat":
                                        if (y != half)
                                        {
                                            SetTile(x, y, z, (byte)((y >= half) ? Block.air : Block.dirt));
                                        }
                                        else
                                        {
                                            SetTile(x, y, z, Block.grass);
                                        }
                                        break;

                                    case "pixel":
                                        if (y == 0)
                                        {
                                            SetTile(x, y, z, Block.blackrock);
                                        }
                                        else
                                            if (x == 0 || x == width - 1 || z == 0 || z == height - 1)
                                            {
                                                SetTile(x, y, z, Block.white);
                                            }

                                        break;
                                }
                                //blocks[x + width * z + width * height * y] = b;
                            }
                        }
                    }
                    break;

                case "island":
                case "mountains":
                case "ocean":
                case "forest":
                    Server.MapGen.GenerateMap(this, type);
                    break;

                case "empty":
                default:
                    break;
            }

            spawnx = (ushort)(width / 2);
            spawny = (ushort)(depth * 0.75f);
            spawnz = (ushort)(height / 2);
            rotx = 0; roty = 0;
            Server.Log("Level initialized");
        }

        public byte GetTile(ushort x, ushort y, ushort z)
        {
            //if (PosToInt(x, y, z) >= blocks.Length) { return null; }
            //Avoid internal overflow
			if (x < 0) { return Block.Zero; }
			if (x >= width) { return Block.Zero; }
			if (y < 0) { return Block.Zero; }
			if (y >= depth) { return Block.Zero; }
            if (z < 0) { return Block.Zero; }
			if (z >= height) { return Block.Zero; }
            return blocks[PosToInt(x, y, z)];
        }
        public byte GetTile(int b)
        {
            ushort x = 0, y = 0, z = 0;
            IntToPos(b, out x, out y, out z);
            return GetTile(x, y, z);
        }

		public void SetTile(ushort x, ushort y, ushort z, byte type)
		{
			blocks[x + width * z + width * height * y] = type;
		}

        public void Blockchange(Player p, ushort x, ushort y, ushort z, byte type) { Blockchange(p, x, y, z, type, true); }
        public void Blockchange(Player p, ushort x, ushort y, ushort z, byte type, bool addaction)
        {
            try
            {
                if (x >= width || y > depth || z >= height) { p.Kick("Building outside boundaries!"); return; }
                if (y == depth) { return; } 
                byte b = GetTile(x, y, z);

                if (Block.Convert(b) != Block.Convert(type))    //Should save bandwidth sending identical looking blocks, like air/op_air changes.
                {
                    Player.GlobalBlockchange(this, x, y, z, type);
                }

                if (b == Block.sponge && physics > 0 && type != Block.sponge)
                {
                    PhysSpongeRemoved(PosToInt(x, y, z));
                }

                SetTile(x, y, z, type);               //Updates server level blocks

                if (physics > 0)
                {
                    if (Block.Physics(type))
                    {
                        AddCheck(PosToInt(x, y, z));
                    }
                }
                changed = true;
            }
            catch
            {
                Server.ErrorLog(p.name + " has triggered a block change error in level.cs on " + name);
                Player.GlobalMessageOps(p.name + " has triggered a block change error in level.cs on " + name);
                IRCBot.Say(p.name + " has triggered a block change error in level.cs on " + name);
            }

            //if (addaction)
            //{
            //    if (edits.Count == edits.Capacity) { edits.Capacity += 1024; }
            //    if (p.actions.Count == p.actions.Capacity) { p.actions.Capacity += 128; }
            //    if (b.lastaction.Count == 5) { b.lastaction.RemoveAt(0); }
            //    Edit foo = new Edit(this); foo.block = b; foo.from = p.name;
            //    foo.before = b.type; foo.after = type;
            //    b.lastaction.Add(foo); edits.Add(foo); p.actions.Add(foo);
            //} b.type = type;
        }
        public void Blockchange(ushort x, ushort y, ushort z, byte type)    //Block change made by physics
        {
            if (y == depth) { return; }
            byte b = GetTile(x, y, z);
            if (Block.Convert(b) != Block.Convert(type))    //Should save bandwidth sending identical looking blocks, like air/op_air changes.
            {
                Player.GlobalBlockchange(this, x, y, z, type);
            }

            if (b == Block.sponge && physics > 0 && type != Block.sponge)
            {
                PhysSpongeRemoved(PosToInt(x, y, z));
            }

            SetTile(x, y, z, type);               //Updates server level blocks

            if (physics > 0)
            {
                if (Block.Physics(type))
                {
                    AddCheck(PosToInt(x, y, z));
                }
            }
        }

        public void Save()
        {
            string path = "levels/" + name + ".lvl";
            try
            {
                //McLevel mclevel = new McLevel();
                //mclevel.Map.Width = (short)width;
                //mclevel.Map.Height = (short)depth;
                //mclevel.Map.Length = (short)height;

                //for (ushort x = 0; x < width; x++)
                //{
                //    for (ushort z = 0; z < height; z++)
                //    {
                //        for (ushort y = 0; y < depth; y++)
                //        {
                //            mclevel.Map.Blocks[x, y, z] = blocks[PosToInt(x, y, z)].type;
                //        }
                //    }
                //}
                //mclevel.SaveFile("mclevels/" + name + ".mclevel");

                if (!Directory.Exists("levels")) { Directory.CreateDirectory("levels"); }

                FileStream fs = File.Create(path);
                GZipStream gs = new GZipStream(fs, CompressionMode.Compress);

                byte[] header = new byte[16];
                BitConverter.GetBytes(1874).CopyTo(header, 0);
                gs.Write(header, 0, 2);
                
                BitConverter.GetBytes(width).CopyTo(header, 0);
                BitConverter.GetBytes(height).CopyTo(header, 2);
                BitConverter.GetBytes(depth).CopyTo(header, 4);
                BitConverter.GetBytes(spawnx).CopyTo(header, 6);
                BitConverter.GetBytes(spawnz).CopyTo(header, 8);
                BitConverter.GetBytes(spawny).CopyTo(header, 10);
                header[12] = rotx; header[13] = roty;
                header[14] = (byte)permissionvisit;
                header[15] = (byte)permissionbuild;
                gs.Write(header, 0, header.Length);
                byte[] level = new byte[blocks.Length];
                for (int i = 0; i < blocks.Length; ++i)
                {
                    if (blocks[i] < 200)
                    {
                        level[i] = blocks[i];
                    }
                    else
                    {
                        level[i] = Block.SaveConvert(blocks[i]);
                    }
                } gs.Write(level, 0, level.Length); gs.Close();
                Server.Log("SAVED: Level \"" + name + "\". " + Player.players.Count + "/" + Properties.players);
                changed = false;
                try
                {
                    File.Copy(path, path + ".backup", true);
                    Server.Log("And backed up");
                }
                catch
                {
                    Server.Log("Failed to make backup");
                }

            }
            catch
            {
                Server.Log("FAILED TO SAVE :" + name);
                Player.GlobalMessage("FAILED TO SAVE :" + name);
                return;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        public bool Backup(string path)
        {
            string BackPath = path + "/" + name + ".lvl";
            string current = "levels/" + name + ".lvl";
            try
            {
                File.Copy(current, BackPath, true);
                return true;            
            }
            catch
            {
                Server.Log("FAILED TO INCREMENTAL BACKUP :" + name);
                Player.GlobalMessage("FAILED TO INCREMENTAL BACKUP :" + name);
                return false;
            }
        }

        public static Level Load(string name){ return Load(name, 0); }
        public static Level Load(string name, byte phys)
        {
            string path = "levels/" + name + ".lvl";
            if (File.Exists(path))
            {
                
                FileStream fs = File.OpenRead(path);
                try
                {
                    GZipStream gs = new GZipStream(fs, CompressionMode.Decompress);
                    byte[] ver = new byte[2];
                    gs.Read(ver, 0, ver.Length);
                    ushort version = BitConverter.ToUInt16(ver, 0);
                    Level level;
                    if (version == 1874)
                    {
                        byte[] header = new byte[16]; gs.Read(header, 0, header.Length);
                        ushort width = BitConverter.ToUInt16(header, 0);
                        ushort height = BitConverter.ToUInt16(header, 2);
                        ushort depth = BitConverter.ToUInt16(header, 4);
                        level = new Level(name, width, depth, height, "empty");
                        level.spawnx = BitConverter.ToUInt16(header, 6);
                        level.spawnz = BitConverter.ToUInt16(header, 8);
                        level.spawny = BitConverter.ToUInt16(header, 10);
                        level.rotx = header[12]; level.roty = header[13];
                        level.permissionvisit = (LevelPermission)header[14];
                        level.permissionbuild = (LevelPermission)header[15];
                    }
                    else
                    {
                        byte[] header = new byte[12]; gs.Read(header, 0, header.Length);
                        ushort width = version;
                        ushort height = BitConverter.ToUInt16(header, 0);
                        ushort depth = BitConverter.ToUInt16(header, 2);
                        level = new Level(name, width, depth, height, "grass");
                        level.spawnx = BitConverter.ToUInt16(header, 4);
                        level.spawnz = BitConverter.ToUInt16(header, 6);
                        level.spawny = BitConverter.ToUInt16(header, 8);
                        level.rotx = header[10]; level.roty = header[11];
                    }

                    level.physics = phys;

                    byte[] blocks = new byte[level.width * level.height * level.depth];
                    gs.Read(blocks, 0, blocks.Length);
                    for (int i = 0; i < level.width * level.height * level.depth; ++i)
                    {
                        level.blocks[i] = blocks[i];
                    } gs.Close(); Server.Log("LOADED: Level \"" + name + "\"."); return level;
                }
                catch (Exception ex) { Server.Log("ERROR loading level \"" + name + "\"."); Server.Log(ex.StackTrace); return null; }
                finally { fs.Close(); }
            }
            else { Server.Log("ERROR loading level \"" + name + "\"."); return null; }
        }

        public int PosToInt(ushort x, ushort y, ushort z)
        {
            if (x < 0) { return -1; }
            if (x >= width) { return -1; }
            if (y < 0) { return -1; }
            if (y >= depth) { return -1; }
            if (z < 0) { return -1; }
            if (z >= height) { return -1; }
            return x + z * width + y * width * height;
			//alternate method: (h * widthY + y) * widthX + x;
        }
        public void IntToPos(int pos, out ushort x, out ushort y, out ushort z)
        {
            y = (ushort)(pos / width / height); pos -= y * width * height;
            z = (ushort)(pos / width); pos -= z * width; x = (ushort)pos;
        }
        public int IntOffset(int pos, int x, int y, int z)
        {
            return pos + x + z * width + y * width * height;
        }

        #region ==Physics==
        public void CalcPhysics()
        {
            try
            {
                if (physics > 0)
                {
                    ushort x, y, z;
                    if (physics == 3)       //Kakashis crazy code that I dont even know what it does.
                    {
                        ListCheck.ForEach(delegate(Check c)
                        {
                            IntToPos(c.b, out x, out y, out z);
                            if (PhysSand(c.b, 13))
                            {
                                if (GetTile((ushort)(x + 1), y, z) != Block.Zero) { PhysAir(PosToInt((ushort)(x + 1), y, z)); }
                                if (GetTile((ushort)(x - 1), y, z) != Block.Zero) { PhysAir(PosToInt((ushort)(x - 1), y, z)); }
                                if (GetTile(x, y, (ushort)(z + 1)) != Block.Zero) { PhysAir(PosToInt(x, y, (ushort)(z + 1))); }
                                if (GetTile(x, y, (ushort)(z - 1)) != Block.Zero) { PhysAir(PosToInt(x, y, (ushort)(z - 1))); }
                                if (GetTile(x, (ushort)(y + 1), z) != Block.Zero) { PhysAir(PosToInt(x, (ushort)(y + 1), z)); }   //Check block above
                            }
                            c.time = 255;
                        });
                    }

                    ListCheck.ForEach(delegate(Check C)    //checks though each block to be updated
                    {
                        try
                        {
                            IntToPos(C.b, out x, out y, out z);
                            switch (blocks[C.b])
                            {
                                case Block.air:         //Placed air
                                    //initialy checks if block is valid
                                    PhysAir(PosToInt((ushort)(x + 1), y, z));
                                    PhysAir(PosToInt((ushort)(x - 1), y, z));
                                    PhysAir(PosToInt(x, y, (ushort)(z + 1)));
                                    PhysAir(PosToInt(x, y, (ushort)(z - 1)));
                                    PhysAir(PosToInt(x, (ushort)(y + 1), z));  //Check block above the air

                                    //Edge of map water
                                    if (y < depth / 2 && y >= (depth / 2) - 2)
                                    {
                                        if (x == 0 || x == width - 1 || z == 0 || z == height - 1)
                                        {
                                            AddUpdate(C.b, Block.water);
                                        }
                                    }

                                    C.time = 255;
                                    break;


                                case Block.dirt:     //Dirt
                                    if (C.time > 80)   //To grass
                                    {
                                        if (Block.LightPass(GetTile(x, (ushort)(y + 1), z)))
                                        {
                                            AddUpdate(C.b, Block.grass);
                                        }
                                        C.time = 255;
                                    }
                                    else
                                    {
                                        C.time++;
                                    }
                                    break;

                                case Block.water:         //Active_water
                                    //initialy checks if block is valid
                                    if (!PhysSpongeCheck(C.b))
                                    {
                                        if (GetTile(x, (ushort)(y + 1), z) != Block.Zero) { PhysSandCheck(PosToInt(x, (ushort)(y + 1), z)); }
                                        PhysWater(PosToInt((ushort)(x + 1), y, z));
                                        PhysWater(PosToInt((ushort)(x - 1), y, z));
                                        PhysWater(PosToInt(x, y, (ushort)(z + 1)));
                                        PhysWater(PosToInt(x, y, (ushort)(z - 1)));
                                        PhysWater(PosToInt(x, (ushort)(y - 1), z));
                                    }
                                    else
                                    {
                                        AddUpdate(C.b, Block.air);  //was placed near sponge
                                    }
                                    C.time = 255;
                                    break;

                                case Block.lava:         //Active_lava
                                    //initialy checks if block is valid
                                    if (C.time >= 4)
                                    {
                                        PhysLava(PosToInt((ushort)(x + 1), y, z), Block.lava);
                                        PhysLava(PosToInt((ushort)(x - 1), y, z), Block.lava);
                                        PhysLava(PosToInt(x, y, (ushort)(z + 1)), Block.lava);
                                        PhysLava(PosToInt(x, y, (ushort)(z - 1)), Block.lava);
                                        PhysLava(PosToInt(x, (ushort)(y - 1), z), Block.lava);
                                        C.time = 255;
                                    }
                                    else
                                    { C.time++; }
                                    break;

                                case Block.sand:    //Sand
                                    if (PhysSand(C.b, Block.sand))
                                    {
                                        PhysAir(PosToInt((ushort)(x + 1), y, z));
                                        PhysAir(PosToInt((ushort)(x - 1), y, z));
                                        PhysAir(PosToInt(x, y, (ushort)(z + 1)));
                                        PhysAir(PosToInt(x, y, (ushort)(z - 1)));
                                        PhysAir(PosToInt(x, (ushort)(y + 1), z));   //Check block above
                                    }
                                    C.time = 255;
                                    break;

                                case Block.gravel:    //Gravel
                                    if (PhysSand(C.b, Block.gravel))
                                    {
                                        PhysAir(PosToInt((ushort)(x + 1), y, z));
                                        PhysAir(PosToInt((ushort)(x - 1), y, z));
                                        PhysAir(PosToInt(x, y, (ushort)(z + 1)));
                                        PhysAir(PosToInt(x, y, (ushort)(z - 1)));
                                         PhysAir(PosToInt(x, (ushort)(y + 1), z));   //Check block above
                                    }
                                    C.time = 255;
                                    break;

                                case Block.sponge:    //SPONGE
                                    PhysSponge(C.b);
                                    C.time = 255;
                                    break;

                                //Adv physics updating anything placed next to water or lava
                                case Block.wood:     //Wood to die in lava
                                case Block.shrub:     //Tree and plants follow
                                case Block.trunk:    //Wood to die in lava
                                case Block.leaf:    //Bushes die in lava
                                case Block.yellowflower:
                                case Block.redflower:
                                case Block.mushroom:
                                case Block.redmushroom:
                                case Block.bookcase:    //bookcase
                                    if (physics == 2)   //Adv physics kills flowers and mushroos in water/lava
                                    {
                                        PhysAir(PosToInt((ushort)(x + 1), y, z));
                                        PhysAir(PosToInt((ushort)(x - 1), y, z));
                                        PhysAir(PosToInt(x, y, (ushort)(z + 1)));
                                        PhysAir(PosToInt(x, y, (ushort)(z - 1)));
                                        PhysAir(PosToInt(x, (ushort)(y + 1), z));   //Check block above
                                    }
                                    C.time = 255;
                                    break;

                                case Block.staircasestep:
                                    PhysStair(C.b);
                                    C.time = 255;
                                    break;

                                case Block.wood_float:   //wood_float
                                    PhysFloatwood(C.b);
                                    C.time = 255;
                                    break;

                                case Block.lava_fast:         //lava_fast
                                    //initialy checks if block is valid
                                    PhysLava(PosToInt((ushort)(x + 1), y, z), Block.lava_fast);
                                    PhysLava(PosToInt((ushort)(x - 1), y, z), Block.lava_fast);
                                    PhysLava(PosToInt(x, y, (ushort)(z + 1)), Block.lava_fast);
                                    PhysLava(PosToInt(x, y, (ushort)(z - 1)), Block.lava_fast);
                                    PhysLava(PosToInt(x, (ushort)(y - 1), z), Block.lava_fast);
                                    C.time = 255;
                                    break;

                                //Special blocks that are not saved
                                case Block.air_flood:   //air_flood
                                    if (C.time < 1)
                                    {
                                        PhysAirFlood(PosToInt((ushort)(x + 1), y, z),Block.air_flood);
                                        PhysAirFlood(PosToInt((ushort)(x - 1), y, z), Block.air_flood);
                                        PhysAirFlood(PosToInt(x, y, (ushort)(z + 1)), Block.air_flood);
                                        PhysAirFlood(PosToInt(x, y, (ushort)(z - 1)), Block.air_flood);
                                        PhysAirFlood(PosToInt(x, (ushort)(y - 1), z), Block.air_flood);
                                        PhysAirFlood(PosToInt(x, (ushort)(y + 1), z), Block.air_flood);

                                        C.time++;
                                    }
                                    else
                                    {
                                        AddUpdate(C.b, 0);    //Turn back into normal air
                                        C.time = 255;
                                    }
                                    break;

                                case Block.door_air:   //door_air         Change any door blocks nearby into door_air
                                    if (C.time == 0)
                                    {
                                        PhysReplace(PosToInt((ushort)(x + 1), y, z), Block.door, Block.door_air);
                                        PhysReplace(PosToInt((ushort)(x - 1), y, z), Block.door, Block.door_air);
                                        PhysReplace(PosToInt(x, y, (ushort)(z + 1)), Block.door, Block.door_air);
                                        PhysReplace(PosToInt(x, y, (ushort)(z - 1)), Block.door, Block.door_air);
                                        PhysReplace(PosToInt(x, (ushort)(y - 1), z), Block.door, Block.door_air);
                                        PhysReplace(PosToInt(x, (ushort)(y + 1), z), Block.door, Block.door_air);
                                    }
                                    if (C.time < 16)
                                    {
                                        C.time++;
                                    }
                                    else
                                    {
                                        AddUpdate(C.b, Block.door);    //turn back into door
                                        C.time = 255;
                                    }
                                    break;

                                case Block.door2_air:   //door_air         Change any door blocks nearby into door_air
                                    if (C.time == 0)
                                    {
                                        PhysReplace(PosToInt((ushort)(x + 1), y, z), Block.door2, Block.door2_air);
                                        PhysReplace(PosToInt((ushort)(x - 1), y, z), Block.door2, Block.door2_air);
                                        PhysReplace(PosToInt(x, y, (ushort)(z + 1)), Block.door2, Block.door2_air);
                                        PhysReplace(PosToInt(x, y, (ushort)(z - 1)), Block.door2, Block.door2_air);
                                        PhysReplace(PosToInt(x, (ushort)(y - 1), z), Block.door2, Block.door2_air);
                                        PhysReplace(PosToInt(x, (ushort)(y + 1), z), Block.door2, Block.door2_air);
                                    }
                                    if (C.time < 16)
                                    {
                                        C.time++;
                                    }
                                    else
                                    {
                                        AddUpdate(C.b, Block.door2);    //turn back into door
                                        C.time = 255;
                                    }
                                    break;

                                case Block.door3_air:   //door_air         Change any door blocks nearby into door_air
                                    if (C.time == 0)
                                    {
                                        PhysReplace(PosToInt((ushort)(x + 1), y, z), Block.door3, Block.door3_air);
                                        PhysReplace(PosToInt((ushort)(x - 1), y, z), Block.door3, Block.door3_air);
                                        PhysReplace(PosToInt(x, y, (ushort)(z + 1)), Block.door3, Block.door3_air);
                                        PhysReplace(PosToInt(x, y, (ushort)(z - 1)), Block.door3, Block.door3_air);
                                        PhysReplace(PosToInt(x, (ushort)(y - 1), z), Block.door3, Block.door3_air);
                                        PhysReplace(PosToInt(x, (ushort)(y + 1), z), Block.door3, Block.door3_air);
                                    }
                                    if (C.time < 16)
                                    {
                                        C.time++;
                                    }
                                    else
                                    {
                                        AddUpdate(C.b, Block.door3);    //turn back into door
                                        C.time = 255;
                                    }
                                    break;

                                case Block.air_flood_layer:   //air_flood_layer
                                    if (C.time < 1)
                                    {
                                        PhysAirFlood(PosToInt((ushort)(x + 1), y, z), Block.air_flood_layer);
                                        PhysAirFlood(PosToInt((ushort)(x - 1), y, z), Block.air_flood_layer);
                                        PhysAirFlood(PosToInt(x, y, (ushort)(z + 1)), Block.air_flood_layer);
                                        PhysAirFlood(PosToInt(x, y, (ushort)(z - 1)), Block.air_flood_layer);

                                        C.time++;
                                    }
                                    else
                                    {
                                        AddUpdate(C.b, 0);    //Turn back into normal air
                                        C.time = 255;
                                    }
                                    break;

                                case Block.air_flood_down:   //air_flood_down
                                    if (C.time < 1)
                                    {
                                        PhysAirFlood(PosToInt((ushort)(x + 1), y, z), Block.air_flood_down);
                                        PhysAirFlood(PosToInt((ushort)(x - 1), y, z), Block.air_flood_down);
                                        PhysAirFlood(PosToInt(x, y, (ushort)(z + 1)), Block.air_flood_down);
                                        PhysAirFlood(PosToInt(x, y, (ushort)(z - 1)), Block.air_flood_down);
                                        PhysAirFlood(PosToInt(x, (ushort)(y - 1), z), Block.air_flood_down);

                                        C.time++;
                                    }
                                    else
                                    {
                                        AddUpdate(C.b, 0);    //Turn back into normal air
                                        C.time = 255;
                                    }
                                    break;

                                case Block.air_flood_up:   //air_flood_up
                                    if (C.time < 1)
                                    {
                                        PhysAirFlood(PosToInt((ushort)(x + 1), y, z), Block.air_flood_up);
                                        PhysAirFlood(PosToInt((ushort)(x - 1), y, z), Block.air_flood_up);
                                        PhysAirFlood(PosToInt(x, y, (ushort)(z + 1)), Block.air_flood_up);
                                        PhysAirFlood(PosToInt(x, y, (ushort)(z - 1)), Block.air_flood_up);
                                        PhysAirFlood(PosToInt(x, (ushort)(y + 1), z), Block.air_flood_up);

                                        C.time++;
                                    }
                                    else
                                    {
                                        AddUpdate(C.b, 0);    //Turn back into normal air
                                        C.time = 255;
                                    }
                                    break;

                                default:    //non special blocks are then ignored, maybe it would be better to avoid getting here and cutting down the list
                                    C.time = 255;
                                    break;
                            }
                        }
                        catch
                        {
                            ListCheck.Remove(C);
                            //Server.Log("Phys check issue");
                        }

                    });

                    ListCheck.RemoveAll(Check => Check.time == 255);  //Remove all that are finished with 255 time

                    ListUpdate.ForEach(delegate(Update C)    //checks though each block to be updated and does so
                    {
                        try
                        {
                            IntToPos(C.b, out x, out y, out z);
                            Blockchange(x, y, z, C.type);
                        }
                        catch
                        {
                            Server.Log("Phys update issue");
                        }
                    });

                    ListUpdate.Clear();

                }
            }
            catch
            {
                Server.Log("Level physics error");
            }
        }
        private void AddCheck(int b)
        {
            try
            {
                if (!ListCheck.Exists(Check => Check.b == b))  //Checks to see if block is already due for a check
                {
                    ListCheck.Add(new Check(b));    //Adds block to list to be updated
                }
            }
            catch
            {
                //Server.Log("Warning-PhysicsCheck");
                //ListCheck.Add(new Check(b));    //Lousy back up plan
            }

            
        }
        private void AddUpdate(int b, int type)
        {
            try
            {
                if (!ListUpdate.Exists(Update => Update.b == b))  //Checks to see if block is already due for an update
                {
                    ListUpdate.Add(new Update(b, (byte)type));
                }
                else
                {
                    if (type == 12 || type == 13)   //Sand and gravel overide
                    {
                        ListUpdate.RemoveAll(Update => Update.b == b);
                        ListUpdate.Add(new Update(b, (byte)type));
                    }
                }
            }
            catch
            {
                //Server.Log("Warning-PhysicsUpdate");
                //ListUpdate.Add(new Update(b, (byte)type));    //Lousy back up plan
            }
        }
        public void ClearPhysics()
        {
            ushort x, y, z;
            ListCheck.ForEach(delegate(Check C)    //checks though each block
            {
                IntToPos(C.b, out x, out y, out z);
                //attemps on shutdown to change blocks back into normal selves that are active, hopefully without needing to send into to clients.
                switch (blocks[C.b])
                {
                    case 200:
                    case 202:
                    case 203:
                        blocks[C.b] = 0;
                        break;
                    case 201:
                        //blocks[C.b] = 111;
                        Blockchange( x, y, z, 111);
                        break;
                    case 205:
                        //blocks[C.b] = 113;
                        Blockchange( x, y, z, 113);
                        break;
                    case 206:
                        //blocks[C.b] = 114;
                        Blockchange( x, y, z, 114);
                        break;
                }
            });

            ListCheck.Clear();
            ListUpdate.Clear();
        }
        //================================================================================================================
        private void PhysWater(int b)
        {
            if (b == -1) { return; }
            switch (blocks[b])
            {
                case 0:
                    if (!PhysSpongeCheck(b)) 
                    {
                        AddUpdate(b, 8);
                    }
                    break;

                case 10:    //hit active_lava
                case 112:    //hit lava_fast
                    if (!PhysSpongeCheck(b)) { AddUpdate(b, 1); }
                    break;

                case 6:
                case 37:
                case 38:
                case 39:
                case 40:
                    if (physics == 2)   //Adv physics kills flowers and mushrooms in water
                    {
                        if (!PhysSpongeCheck(b)) { AddUpdate(b, 0); }
                    }
                    break;

                case 12:    //sand
                case 13:    //gravel
                case 110:   //woodfloat
                    AddCheck(b);
                    break;

                default:
                    break;
            }
        }
        //================================================================================================================
        private void PhysLava(int b, byte type)
        {
            if (b == -1) { return; }
            switch (blocks[b])
            {
                case 0:
                    AddUpdate(b, type);
                    break;

                case 8:    //hit active_water
                    AddUpdate(b, 1);
                    break;

                case 12:    //sand
                    if (physics == 2)   //Adv physics changes sand to glass next to lava
                    {
                        AddUpdate(b, 20);
                    }
                    else
                    {
                        AddCheck(b);
                    }
                    break;

                case 13:    //gravel
                    AddCheck(b);
                    break;

                case 5:
                case 6:
                case 17:
                case 18:
                case 37:
                case 38:
                case 39:
                case 40:
                    if (physics == 2)   //Adv physics kills flowers and mushrooms plus wood in lava
                    {
                        AddUpdate(b, 0);
                    }
                    break;

                default:
                    break;
            }
        }
        //================================================================================================================
        private void PhysAir(int b)
        {
            if (b == -1) { return; }
            switch (blocks[b])
            {
                case 8:     //active water
                case 10:    //active_lava
                case 12:    //sand
                case 13:    //gravel
                case 110:   //wood_float
                case 112:   //lava_fast
                    AddCheck(b);
                    break;

                default:
                    break;
            }
        }
        //================================================================================================================
        private bool PhysSand(int b, byte type)   //also does gravel
        {
            if (b == -1) { return false; }
            int tempb = b;
            bool blocked = false;
            bool moved = false;

            do
            {
                tempb = IntOffset(tempb, 0, -1, 0);     //Get block below each loop
				if (GetTile(tempb) != Block.Zero) 
                {
                    switch (blocks[tempb])
                    {
                        case 0:         //air lava water
                        case 8:
                        case 10:
                            moved = true;
                            break;

                        case 6:
                        case 37:
                        case 38:
                        case 39:
                        case 40:
                            if (physics == 2)   //Adv physics crushes plants with sand
                            {moved = true;}
                            else
                            {blocked = true;}
                            break;

                        default:
                            blocked = true;
                            break;
                    }
                    if (physics == 2){blocked = true;}
                } 
                else                    
                {blocked = true;}
            }    
            while (!blocked);

            if (moved)
            {
                AddUpdate(b, 0);
                if (physics == 2)
                {AddUpdate(tempb, type);}
                else
                {AddUpdate(IntOffset(tempb,0,1,0), type);}
            }

            return moved;
        }

        private void PhysSandCheck(int b)   //also does gravel
        {
            if (b == -1) { return; }
            switch (blocks[b])
            {
                case 12:    //sand
                case 13:    //gravel
                case 110:   //wood_float
                    AddCheck(b);
                    break;

                default:
                    break;
            }
        }
        //================================================================================================================
        private void PhysStair(int b)
        {
           int tempb = IntOffset(b, 0, -1, 0);     //Get block below
		   if (GetTile(tempb) != Block.Zero)
           {
               if (GetTile(tempb) == Block.staircasestep)
               {
                   AddUpdate(b, 0);
                   AddUpdate(tempb, 43);
               }
           }
        }
        //================================================================================================================
        private bool PhysSpongeCheck(int b)         //return true if sponge is near
        {
            int temp = 0;
            for (int x = -2; x <= +2; ++x){
                for (int y = -2; y <= +2; ++y){
                    for (int z = -2; z <= +2; ++z){
                        temp = IntOffset(b, x, y, z);
						if (GetTile(temp) != Block.Zero)
                        {
                            if (GetTile(temp) == 19) { return true; }
                        }
                    }
                }
            }
            return false;
        }
        //================================================================================================================
        private void PhysSponge(int b)         //turn near water into air when placed
        {
            int temp = 0;
            for (int x = -2; x <= +2; ++x)
            {
                for (int y = -2; y <= +2; ++y)
                {
                    for (int z = -2; z <= +2; ++z)
                    {
                        temp = IntOffset(b, x, y, z);
						if (GetTile(temp) != Block.Zero)
                        {
                            if (GetTile(temp) == 8) { AddUpdate(temp, 0); }
                        }
                    }
                }
            }

        }
        //================================================================================================================
        public void PhysSpongeRemoved(int b)         //Reactivates near water
        {
            //TODO Calc only edge
            int temp = 0;
            for (int x = -3; x <= +3; ++x)
            {
                for (int y = -3; y <= +3; ++y)
                {
                    for (int z = -3; z <= +3; ++z)
                    {
                        temp = IntOffset(b, x, y, z);
						if (GetTile(temp) != Block.Zero)
                        {
                            if (GetTile(temp) == 8) { AddCheck(temp); }
                        }
                    }
                }
            }

        }
        //================================================================================================================
        private void PhysFloatwood(int b)
        {
            int tempb = IntOffset(b, 0, -1, 0);     //Get block below
			if (GetTile(tempb) != Block.Zero)
            {
                if (GetTile(tempb) == 0)
                {
                    AddUpdate(b, 0);
                    AddUpdate(tempb, 110);
                    return;
                }
            }

            tempb = IntOffset(b, 0, 1, 0);     //Get block above
			if (GetTile(tempb) != Block.Zero)
            {
                if (GetTile(tempb) == 8)
                {
                    AddUpdate(b, 8);
                    AddUpdate(tempb, 110);
                    return;
                }
            }
        }
        //================================================================================================================
        private void PhysAirFlood(int b, byte type)
        {
            if (b == -1) { return; }
            switch (blocks[b])
            {
                case 8:
                case 10:
                case 112:   //lava_fast
                    AddUpdate(b, type);
                    break;

                default:
                    break;
            }
        }
        //================================================================================================================
        private void PhysReplace(int b, byte typeA, byte typeB)     //replace any typeA with typeB
        {
            if (b == -1) { return; }
            if (blocks[b] == typeA)
            {
                AddUpdate(b, typeB);
            }
        }
        #endregion

        public static LevelPermission PermissionFromName(string name)
        {
            switch (name.ToLower())
            {
                case "guest":
                    return LevelPermission.Guest;
                case "builder":
                    return LevelPermission.Builder;
                case "advbuilder":
                    return LevelPermission.AdvBuilder;
                case "moderator":
                    return LevelPermission.Moderator;
                case "operator":
                    return LevelPermission.Operator;
                case "admin":
                    return LevelPermission.Admin;
                default:
                    return LevelPermission.Null;
            }
        }

        public static string PermissionToName(LevelPermission perm)
        {
            switch (perm)
            {
                case LevelPermission.Guest:
                    return "Guest";
                case LevelPermission.Builder:
                    return "Builder";
                case LevelPermission.AdvBuilder:
                    return "AdvBuilder";
                case LevelPermission.Moderator:
                    return "Moderator";
                case LevelPermission.Operator:
                    return "Operator";
                case LevelPermission.Admin:
                    return "Admin";
                default:
                    return "Null";
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------------
    public class Check
    {
        public int b;
        public byte time;
        public Check(int b) 
        {
            this.b = b;
            time = 0;
        }
    }
    //-------------------------------------------------------------------------------------------------------------------------------------------------------
    public class Update
    {
        public int b;
        public byte type;
        public Update(int b, byte type)
        {
            this.b = b;
            this.type = type;
        }
    }
}
