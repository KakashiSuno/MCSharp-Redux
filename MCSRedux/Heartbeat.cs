using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System.Collections;

namespace Minecraft_Server
{
	
	public class Heartbeat
	{
		static int _timeout = 60 * 1000;

		static string hash;
		public static string serverURL;
		static string staticVars;

		static BackgroundWorker worker;
		static HttpWebRequest request;


		public static void Init()
		{
			staticVars = "port=" + Properties.port +
						 "&max=" + Properties.players +
						 "&name=" + UrlEncode(Properties.name) +
						 "&public=" + Properties.pub +
						 "&version=" + Properties.version;
			worker = new BackgroundWorker();
			worker.DoWork += new DoWorkEventHandler(worker_DoWork);
			worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

			worker.RunWorkerAsync();
		}

		static void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			Pump(Beat.Minecraft);
			Pump(Beat.fList);
		}

		static void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (!Properties.console && Server.win != null)
				Server.win.UpdateUrl(serverURL);
			Thread.Sleep(_timeout);
			worker.RunWorkerAsync();
		}
		/*
		Minecraft_Server.Heartbeat.Pump(Beat.Minecraft);
		Minecraft_Server.Heartbeat.Pump(Beat.fList);
		*/
		public static bool Pump(Beat type)
		{
			if (staticVars == null)
				Init();
			// default information to send
			string postVars = staticVars;

			string url = "http://www.minecraft.net/heartbeat.jsp";
			try
			{
				int hidden = 0;
				// append additional information as needed
				switch (type)
				{
					case Beat.Minecraft:
						postVars += "&salt=" + Properties.salt;
						goto default;
					case Beat.fList:
						if (hash == null) // we haven't set the hash yet, need to do that before we use this
							throw new Exception("Hash not set");

						// change url to flist
						url = "http://list.fragmer.net/announce.php";

						// build list of current players in server
						if (Player.number > 0)
						{
							string players = "";
							foreach (Player p in Player.players)
							{
								if (p.hidden)
								{
									hidden++;
									continue;
								}
								players += p.name + ",";
							}
							if(Player.number - hidden > 0)
								postVars += "&players=" + players.Substring(0, players.Length - 1);
						}
						string worlds = "";
						foreach (Level l in Server.levels)
						{
							worlds += l.name + ",";
							postVars += "&worlds=" + worlds.Substring(0, worlds.Length - 1);
						}

						// append flist additional params
                        postVars += "&motd=" + UrlEncode(Properties.motd) +
                                "&hash=" + hash +
                                "&data=" + Server.Version + "," + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() +
								"&server=MCSharp";

						goto default;
					case Beat.Day7Tech:
						// do stuff here
						goto default;
					default:
						postVars += "&users=" + (Player.number - hidden);
						break;

				}

				request = (HttpWebRequest)WebRequest.Create(new Uri(url));
				request.Method = "POST";
				request.ContentType = "application/x-www-form-urlencoded";
				request.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
				byte[] formData = Encoding.ASCII.GetBytes(postVars);
				request.ContentLength = formData.Length;
				request.Timeout = 15000;
				try
				{
					using (Stream requestStream = request.GetRequestStream())
					{
						requestStream.Write(formData, 0, formData.Length);
						requestStream.Close();
					}
				}
				catch (WebException e)
				{
					if (e.Status == WebExceptionStatus.Timeout)
					{

						throw new WebException("Failed during request.GetRequestStream()", e.InnerException, e.Status, e.Response);
					}
				}

				if (hash == null)
				{
					using (WebResponse response = request.GetResponse())
					{
						using (StreamReader responseReader = new StreamReader(response.GetResponseStream()))
						{
							string line = responseReader.ReadLine();
							hash = line.Substring(line.LastIndexOf('=') + 1);
							serverURL = line;

							File.WriteAllText("externalurl.txt", serverURL);
						}
					}
				}
                Server.LogConsole(string.Format("Heartbeat: {0}", type));
			}
			catch (WebException e)
			{
				if (e.Status == WebExceptionStatus.Timeout)
				{
					Server.Log(string.Format("Timeout: {0}", type));
				}
				Server.ErrorLog(e);
			}
			catch (Exception e)
			{
				Server.Log(string.Format("Error reporting to {0}", type));
				Server.ErrorLog(e);
				return false;
			}
			finally
			{
				request.Abort();
			}
			return true;
		}

		public static string UrlEncode(string input)
		{
			StringBuilder output = new StringBuilder();
			for (int i = 0; i < input.Length; i++)
			{
				if ((input[i] >= '0' && input[i] <= '9') ||
					(input[i] >= 'a' && input[i] <= 'z') ||
					(input[i] >= 'A' && input[i] <= 'Z') ||
					input[i] == '-' || input[i] == '_' || input[i] == '.' || input[i] == '~')
				{
					output.Append(input[i]);
				}
				else if (Array.IndexOf<char>(reservedChars, input[i]) != -1)
				{
					output.Append('%').Append(((int)input[i]).ToString("X"));
				}
			}
			return output.ToString();
		}
		public static char[] reservedChars = { ' ', '!', '*', '\'', '(', ')', ';', ':', '@', '&',
                                                 '=', '+', '$', ',', '/', '?', '%', '#', '[', ']' };
	}

	public enum Beat
	{
		Minecraft,
		fList,
		Day7Tech
	}
}
