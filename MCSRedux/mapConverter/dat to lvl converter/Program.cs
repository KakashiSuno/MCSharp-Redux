using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.mojang.minecraft.level;
using System.IO;
using System.IO.Compression;


namespace dat_to_lvl_converter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = ".dat to .lvl converter"; // Set the console title

            Console.WriteLine("Choose a file to convert...");
            string file = Console.ReadLine();

            Level dat = null;

            Console.WriteLine("Loading file");
            if (File.Exists(file))
            {
                try { dat = Level.Load(file); }
                catch (Exception e) { Console.WriteLine(e.Message); Console.ReadLine(); return; }

                Console.WriteLine("File loaded");
                Console.Write("Reading map name:");
                string name = dat.name;
                Console.WriteLine(name);
                Console.Write("Reading map width :");
                Int16 width = (Int16)dat.width;
                Console.WriteLine(width);
                Console.Write("Reading map height:");
                Int16 height = (Int16)dat.height;
                Console.WriteLine(height);
                Console.Write("Reading map depth :");
                Int16 depth = (Int16)dat.depth;
                Console.WriteLine(depth);
                Console.Write("Reading map spawn :");
                Int16 spawnx = (Int16)dat.xSpawn;
                Int16 spawny = (Int16)dat.ySpawn;
                Int16 spawnz = (Int16)dat.zSpawn;
                Console.WriteLine(spawnx + " " + spawny + " " + spawnz);
                byte rotx = 0;
                byte roty = 0;
                Console.WriteLine();

                Console.WriteLine("Creating file");
                string path = name + ".lvl";
                FileStream fs = File.Create(path);
                GZipStream gs = new GZipStream(fs, CompressionMode.Compress);
                byte[] header = new byte[14];
                BitConverter.GetBytes(width).CopyTo(header, 0);
                Console.WriteLine("Wrote map width");
                BitConverter.GetBytes(height).CopyTo(header, 2);
                Console.WriteLine("Wrote map height");
                BitConverter.GetBytes(depth).CopyTo(header, 4);
                Console.WriteLine("Wrote map depth");
                BitConverter.GetBytes(spawnx).CopyTo(header, 6);
                BitConverter.GetBytes(spawnz).CopyTo(header, 8);
                BitConverter.GetBytes(spawny).CopyTo(header, 10);
                Console.WriteLine("Wrote map spawn");
                header[12] = rotx; header[13] = roty;
                gs.Write(header, 0, header.Length);
                Console.Write("Number of blocks :");
                Console.WriteLine(dat.blocks.Length);
                byte[] level = new byte[dat.blocks.Length];
                for (int i = 0; i < dat.blocks.Length; ++i)
                {
                    level[i] = dat.blocks[i];
                } gs.Write(level, 0, level.Length); gs.Close();
                Console.Write("Complete, press to continue.");
                Console.Read();
            }
            else
            {
                Console.WriteLine("File {0} does not exist", file);
                Console.Read();
            }
        }
    }
}
