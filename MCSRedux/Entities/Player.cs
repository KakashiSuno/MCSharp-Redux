using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

namespace MCSRedux.Entities
{
    class Player : Entity
    {
		TcpClient cl;
		
		public Player(TcpClient c)
		{
			if(c == null)
			{
				MCSR.log.Write("ERROR: Entity (Player) does not exist!");
			}
		}
    }
}
