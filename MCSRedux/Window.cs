using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;

namespace Minecraft_Server
{
    public partial class Window : Form
    {
        Regex regex = new Regex(@"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\." +
                                "([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$");
        // for cross thread use
        delegate void StringCallback(string s);
        delegate void PlayerListCallback(List<Player> players);
        delegate void ReportCallback(Report r);

        Thread ServerThread;

        public Window()
        {
            InitializeComponent();
        }

        private void Window_Load(object sender, EventArgs e)
        {
            ServerThread = new Thread(new ThreadStart(delegate
            {
                new Server(this);
            }));
            ServerThread.Start();
            this.Text = Properties.name;
        }

        /// <summary>
        /// Does the same as Console.Write() only in the form
        /// </summary>
        /// <param name="s">The string to write</param>
        public void Write(string s)
        {
            if (this.InvokeRequired)
            {
                StringCallback d = new StringCallback(Write);
                this.Invoke(d, new object[] { s });
            }
            else
            {
                txtLog.AppendText(s);
            }
        }
        /// <summary>
        /// Does the same as Console.WriteLine() only in the form
        /// </summary>
        /// <param name="s">The line to write</param>
        public void WriteLine(string s)
        {
            if (this.InvokeRequired)
            {
                StringCallback d = new StringCallback(WriteLine);
                this.Invoke(d, new object[] { s });
            }
            else
            {
                txtLog.AppendText(s + "\r\n");
            }
        }
        /// <summary>
        /// Updates the list of client names in the window
        /// </summary>
        /// <param name="players">The list of players to add</param>
        public void UpdateClientList(List<Player> players)
        {
            if (this.InvokeRequired)
            {
                PlayerListCallback d = new PlayerListCallback(UpdateClientList);
                this.Invoke(d, new object[] { players });
            }
            else
            {
                liClients.Items.Clear();
                Player.players.ForEach(delegate(Player p) { liClients.Items.Add(p.name); });
            }
        }
        /// <summary>
        /// Places the server's URL at the top of the window
        /// </summary>
        /// <param name="s">The URL to display</param>
        public void UpdateUrl(string s)
        {
            if (this.InvokeRequired)
            {
                StringCallback d = new StringCallback(UpdateUrl);
                this.Invoke(d, new object[] { s });
            }
            else
                txtUrl.Text = s;
        }

        public void SendReport(Report r)
        {
            if (this.InvokeRequired)
            {
                ReportCallback d = new ReportCallback(SendReport);
                this.Invoke(d, new object[] { r });
            }
            else
            {
                txtReports.AppendText(String.Format("> {0} : {1]\r\n", r.Name, r.Reason));
            }
        }

        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logger.Dispose();
            ServerThread.Abort();
            Server.Exit();
        }

        private void btnKick_Click(object sender, EventArgs e)
        {
            if (liClients.SelectedIndex >= 0)
            {
                Player p = Player.Find(liClients.SelectedItem.ToString());
                if (p != null)
                {
                    p.Kick("You were kicked by [console]!"); 
                    IRCBot.Say(p.name + " was kicked by [console]!");
                    UpdateClientList(Player.players);
                }
            }
            else
                MessageBox.Show("You need to select someone");
        }

        private void btnBan_Click(object sender, EventArgs e)
        {
            if (liClients.SelectedIndex >= 0)
            {
                string name = liClients.SelectedItem.ToString();
                if (Server.operators.Contains(name)) { Server.operators.Remove(name); Server.operators.Save("admins.txt"); }
                if (Server.builders.Contains(name)) { Server.builders.Remove(name); Server.builders.Save("builders.txt"); }
                if (Server.advbuilders.Contains(name)) { Server.advbuilders.Remove(name); Server.advbuilders.Save("advbuilders.txt"); }
                if (Server.superOps.Contains(name)) { Server.superOps.Remove(name); Server.superOps.Save("uberOps.txt"); }
                if (Server.banned.Contains(name)) { MessageBox.Show(name + " is already banned."); return; }
                Player who = Player.Find(name);
                if (who == null) { Player.GlobalMessage(name + " &f(offline)&e is now &8banned&e!"); }
                else
                {
                    Player.GlobalChat(who, who.color + who.name + "&e is now &8banned&e!", false);
                    who.group = Group.Find("banned"); who.color = who.group.color; Player.GlobalDie(who, false);
                    Player.GlobalSpawn(who, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1], false);
                } Server.banned.Add(name); Server.banned.Save("banned.txt", false); IRCBot.Say(name + " was banned by [console]");
                Server.Log("BANNED: " + name.ToLower());
            }
        }

        private void btnKickban_Click(object sender, EventArgs e)
        {
            if (liClients.SelectedIndex >= 0)
            {
                string name = liClients.SelectedItem.ToString();
                if (Server.operators.Contains(name)) { Server.operators.Remove(name); Server.operators.Save("admins.txt"); }
                if (Server.builders.Contains(name)) { Server.builders.Remove(name); Server.builders.Save("builders.txt"); }
                if (Server.advbuilders.Contains(name)) { Server.advbuilders.Remove(name); Server.advbuilders.Save("advbuilders.txt"); }
                if (Server.superOps.Contains(name)) { Server.superOps.Remove(name); Server.superOps.Save("uberOps.txt"); }
                if (Server.banned.Contains(name)) { MessageBox.Show(name + " is already banned."); return; }
                Player who = Player.Find(name);
                if (who == null) { Player.GlobalMessage(name + " &f(offline)&e is now &8banned&e!"); }
                else
                {
                    who.Kick("You got served!");
                    Player.GlobalMessage("- " + who.color + who.name + "&e is now &8banned&e!");
                    who.group = Group.Find("banned"); who.color = who.group.color; Player.GlobalDie(who, false);
                    Player.GlobalSpawn(who, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1], false);
                } Server.banned.Add(name); Server.banned.Save("banned.txt", false); IRCBot.Say(name + " was kickBanned by [console]");
                Server.Log("BANNED: " + name.ToLower());
            }
            else
                MessageBox.Show("You need to select someone");
        }

        private void btnBanIp_Click(object sender, EventArgs e)
        {
            if (liClients.SelectedIndex >= 0)
            {
                string message = liClients.SelectedItem.ToString();
                Player who = null;
                who = Player.Find(message);
                if (who != null)
                    message = who.ip;

                if (message.Equals("127.0.0.1")) { MessageBox.Show("You can't ip-ban the server!"); return; }
                if (!regex.IsMatch(message)) { MessageBox.Show("Not a valid ip!"); return; }
                if (Server.bannedIP.Contains(message)) { MessageBox.Show(message + " is already ip-banned."); return; }
                Player.GlobalMessage(message + " got &8ip-banned&e!");
                IRCBot.Say("IP-BANNED: " + message.ToLower() + " by console");
                Server.bannedIP.Add(message); Server.bannedIP.Save("banned-ip.txt", false);
                Server.Log("IP-BANNED: " + message.ToLower());
            }
            else
                MessageBox.Show("You need to select someone");
        }

        private void btnChangeRank_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Player.GlobalMessage("[console]: " + txtInput.Text);
                WriteLine("<CONSOLE> " + txtInput.Text);
                txtInput.Clear();
            }
        }
    }
}
