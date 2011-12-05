//  
//  Server.cs
//  
//  Author:
//       MCSR Team <day7tech@gmail.com>
// 
//  Copyright (c) 2011 MCSR Team
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Net;
using System.Net.Sockets;

using MCSRedux.Configuration;
using MCSRedux.Entities;

namespace MCSRedux.Network
{
	public class Server
	{
		TcpListener listen;
		IPEndPoint endpoint;
		
		public Server(string ip, int port)
		{
			IPAddress ipaddr;
			IPAddress.TryParse(Properties.serverip, out ipaddr);
			endpoint = new IPEndPoint((ipaddr != null) ? ipaddr : IPAddress.Any, Properties.serverport);
			
			listen = new TcpListener(endpoint);
		}
		
		public void Listen()
		{
			Listen(0);
		}
		public void Listen(int backlog)
		{
			listen.Start(backlog);
			
			listen.BeginAcceptTcpClient(new AsyncCallback(AcceptConnection), this);
		}
		
		void AcceptConnection(IAsyncResult result)
		{
			TcpClient cl = listen.EndAcceptTcpClient(result);
			
			if(cl != null)
			{
				new McClient(cl);
			}
			
			listen.BeginAcceptTcpClient(new AsyncCallback(AcceptConnection), result.AsyncState);
		}
	}
}

