// Data Node Archive (DNA)
// Copyright Caleb Gibbs

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Minecraft_Server
{
    public enum TableData
    {
        ID,
        IP,
        Rank,
    }
    public class DNA
    {
        static List<string> tables = new List<string>(); // stores the data after opening
        static string name;
        static int MagicNumber { get { return 0x240316; } }

        /// <summary>
        /// Get a value from the database
        /// </summary>
        /// <param name="table">the name of the table (player username)</param>
        /// <param name="data">the data type to get</param>
        /// <returns>A string representing the fetched data</returns>
        public static string GetData(string table, TableData data)
        {
            foreach (string t in tables)
            {
                if (t.Split(':').Length > 1)
                {
                    if (t.Split(':')[0].ToLower() == table.ToLower())
                    {
                        switch (data)
                        {
                            // entrys are stored as 'entryname:ip:rank'
                            // Example: KakashiSuno:127.0.0.1:SuperOp'

                            // This function doesn't return the entry name
                            // because it's assumed that you would know it
                            case TableData.IP:
                                return t.Split(':')[1];
                            case TableData.Rank:
                                return t.Split(':')[2];
                        }
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// Open a DNA database
        /// </summary>
        /// <param name="filename">The name of the file to open</param>
        public static void OpenDatabase(string filename)
        {
            name = filename;
            try
            {
                if (File.Exists(filename))
                {
                    BinaryReader r = new BinaryReader(File.OpenRead(filename));
                    if (r.ReadInt32() == MagicNumber)
                    {
                        for (int x = 0; x < r.ReadInt32(); x++)
                        {
                            if (r.ReadString() == "Entry") // Marks the start of an entry
                            {
                                int count = r.ReadInt32();
                                string n = Encoding.ASCII.GetString(r.ReadBytes(count));
                                count = r.ReadInt32();
                                string i = Encoding.ASCII.GetString(r.ReadBytes(count));
                                count = r.ReadInt32();
                                string g = Encoding.ASCII.GetString(r.ReadBytes(count));
                                tables.Add(n + ":" + i + ":" + g);

                            }
                            if (r.ReadString() == "EndEntry") // Marks the end of an entry
                                continue;
                            else { Server.ErrorLog("Error loading " + filename + ": Entry not valid"); }
                        }
                    }
                    else { Server.ErrorLog("Error loading " + filename + ": Wrong Magic Number"); }
                }
            }
            catch (Exception ex) { Server.ErrorLog(ex); }
        }

        public static void CreateDatabase(string filename)
        {
            BinaryWriter w = new BinaryWriter(File.Create(filename));
            w.Write(MagicNumber);
            w.Write((int)0);
            w.Flush();
            w.Close();
        }

        /// <summary>
        /// Loops through all connected players and writes their respective data
        /// </summary>
        public static void WriteData()
        {
            BinaryWriter w = new BinaryWriter(File.Open(name, FileMode.Create));
            w.Write(MagicNumber);
            int count = Player.players.Count;

            tables.Clear();
            w.Write(count);
            foreach (Player p in Player.players)
            {
                w.Write("Entry");
                w.Write(p.name.Length);
                w.Write(Encoding.ASCII.GetBytes(p.name));
                w.Write(p.ip.Length);
                w.Write(Encoding.ASCII.GetBytes(p.ip));
                w.Write(p.group.name.Length);
                w.Write(Encoding.ASCII.GetBytes(p.group.name));
                w.Write("EndEntry");
                tables.Add(p.name + ":" + p.ip + ":" + p.group.name);
            }
            w.Flush();
            w.Close();
            Player.GlobalMessageOps("DNA written");
        }

        public static bool IsValid(string filename)
        {
            if (File.Exists(filename))
            {
                try
                {
                    BinaryReader r = new BinaryReader(File.OpenRead(filename));
                    if (r.ReadInt32() == MagicNumber)
                        return true;
                    else
                        return false;
                }
                catch { return false; }
                
            }
            return false;
        }
    }
}
