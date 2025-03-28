using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZooBuilderBackend
{
    public class WebSocketConnection
    {
        private List<TcpClient> connections = new List<TcpClient>();

        private bool isActive = true;
        public WebSocketConnection(string ip, int port)
        {
            var server = new TcpListener(IPAddress.Parse(ip), port);
            server.Start();
            Console.WriteLine($"Server started on {ip}:{port}");
            AcceptClient(server);
            BroadcastTime();
        }

        async Task AcceptClient(TcpListener server)
        {
            while (isActive)
            {
                var client = await server.AcceptTcpClientAsync();
                connections.Add(client);
                Console.WriteLine($"Client connected");
            }
        }

        async Task BroadcastTime()
        {
            while (isActive)
            {
                var message = "Hello you!";
                var bytes = Encoding.UTF8.GetBytes(message);
                foreach (var client in connections)
                {
                    if (client.Client.Connected)
                        client.Client.Send(bytes);
                }
                Thread.Sleep(1000);
            }
        }
    }
}