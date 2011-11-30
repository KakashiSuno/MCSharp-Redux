using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

using MCSRedux.Packets;

namespace MCSRedux.Entities
{
    class Player : Entity
    {
		TcpClient cl;
		
		// I should probably explain this.
		// I'm initializing rbuffer to a length of 39 because
		// The first packet that should be recieved from a connecting client
		// is Login Request, which has a maximum length of 39 bytes.
		// After it handles that the server will reinit the array
		// To a more acceptable size
		byte[] rbuffer = new byte[39];
		
		string ip;
		string name;
		
		
		public Player(TcpClient c)
		{
			if(c == null)
			{
				MCSR.log.Write("ERROR: Entity (Player) does not exist!");
			}
			
			ip = c.Client.RemoteEndPoint.ToString().Split(':')[0];			
			cl = c;
			
			cl.GetStream().BeginRead(rbuffer, 0, 39, AcceptPacket, cl);
		}
		
		void AcceptPacket(IAsyncResult result)
		{
			cl.GetStream().EndRead(result);
			Packet.ParseIncoming(rbuffer);
		}
    }
}
