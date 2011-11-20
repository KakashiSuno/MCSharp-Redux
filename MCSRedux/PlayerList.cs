using System;
using System.IO;
using System.Collections.Generic;

namespace Minecraft_Server
{
    public sealed class PlayerList
    {
        List<string> players = new List<string>();
        public PlayerList() { }
        public void Add(string p) { players.Add(p.ToLower()); }
        public bool Remove(string p) { return players.Remove(p.ToLower()); }
        public bool Contains(string p) { return players.Contains(p.ToLower()); }
        public List<string> All() { return new List<string>(players); }
        public void Save(string path) { Save(path, true); }
        public void Save(string path, bool console)
        {
            StreamWriter file = File.CreateText("ranks/" + path);
            players.ForEach(delegate(string p) { file.WriteLine(p); });
            file.Close(); if (console) { Server.Log("SAVED: " + path); }
        }
        public static PlayerList Load(string path)
        {
            if (!Directory.Exists("ranks")) { Directory.CreateDirectory("ranks"); }
            path = "ranks/" + path;
            PlayerList list = new PlayerList();
            if (File.Exists(path))
            {
                Server.Log("LOADED: " + path);
                foreach (string line in File.ReadAllLines(path)) { list.Add(line); }
            }
            else 
            { 
                File.Create(path).Close();
                Server.Log("CREATED NEW: " + path);
            } return list;
        }
    }
}