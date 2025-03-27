using System;
using System.Net;
using System.Net.Sockets;

namespace ZooBuilderBackend
{
    public class WebSocketConnection
    {
        public WebSocketConnection(string ip, int port)
        {
            var server = new TcpListener(IPAddress.Parse(ip), port);
            server.Start();
            Console.WriteLine($"Server started on {ip}:{port}");
            var client = server.AcceptTcpClient();
            Console.WriteLine($"Client connected");
        }
    }
}