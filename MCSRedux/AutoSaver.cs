using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.ComponentModel;


namespace Minecraft_Server
{
    class AutoSaver
    {
        static int _interval;
        const string backupPath = "levels/backups";
        static BackgroundWorker worker;
        static int count = 1;
        public AutoSaver(int interval)
        {
            _interval = interval * 1000;

            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.RunWorkerAsync();
        }

        static void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Run();
        }
        static void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Thread.Sleep(_interval);
            worker.RunWorkerAsync();
        }

        static void Run()
        {

            try
            {
                count--;

                foreach (Level l in Server.levels)
                {
                    try
                    {
                        if (l.changed) { l.Save(); }

                        if (count == 0)
                        {
                            int backupNumber = 1;
                            if (Directory.Exists(backupPath + "/" + l.name))
                            { backupNumber = Directory.GetDirectories(backupPath + "/" + l.name).Length + 1; }
                            else
                            { Directory.CreateDirectory(backupPath + "/" + l.name); }
                            string path = backupPath + "/" + l.name + "/" + backupNumber;
                            Directory.CreateDirectory(path);
                            if (l.Backup(path))
                            {
                                foreach (Player p in Player.players)
                                {
                                    if (p.level == l)
                                        p.SendMessage("Backup " + backupNumber + " saved.");
                                }
                                Server.Log("Backup " + backupNumber + " saved for " + l.name);
                            }
                        }
                    }
                    catch
                    {
                        Server.Log("Backup for " + l.name + " has caused an error.");
                    }
                }

                if (count <= 0)
                {
                    count = 15;
                }
            }
            catch (Exception e) { Server.ErrorLog(e); }
        }
    }
}
