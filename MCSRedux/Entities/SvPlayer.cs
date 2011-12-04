using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

using Substrate;

using MCSRedux.Maps;
using MCSRedux.Network;
using MCSRedux.Network.Packets;

namespace MCSRedux.Entities
{
    class SvPlayer : Substrate.Player
    {
		TcpClient client;
		string ip;
		
		byte[] buffer = new byte[19];
		
		public SvPlayer(TcpClient cl)
		{
			client = cl;
			ip = client.Client.RemoteEndPoint.ToString().Split(':')[0];
			MCSR.log.Write(LogType.Message, "Player connection: " + ip);
			
			string s = TypeHandler.ReadString(client.GetStream());
			MCSR.log.Write(LogType.Message, "Player Identified as: " + s);
		}
    }
}
