using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

using System.Threading;

using Substrate;

using MCSRedux.Maps;
using MCSRedux.Network;
using MCSRedux.Network.Packets;

namespace MCSRedux.Entities
{
    public partial class SvPlayer : Substrate.Player
    {
		McClient client;
		
		int id;		
		public string IP { get{ return client.ip; }}
		
		Thread receiveThread;
		
		public SvPlayer(McClient cl)
		{
			client = cl;
			MCSR.log.Write(LogType.Message, "Player connection: " + IP);
			
			this.Name = TypeHandler.ReadString(client.GetStream());
			MCSR.log.Write(LogType.Message, IP + " identified as: " + Name);
			
			byte[] tmp = new byte[5];
			tmp[0] = (byte)PacketID.Handshake;
			Array.Copy(TypeHandler.GetBytes((short)1), 0, tmp, 1, 2);
			Array.Copy(TypeHandler.GetBytes("-"), 0, tmp, 3, 2);
			
			SendMap(MCSR.World);
			
			receiveThread = new Thread(Receive);
			receiveThread.Start();
		}
		
		public void Kick(string msg)
		{
			
		}
    }
}
