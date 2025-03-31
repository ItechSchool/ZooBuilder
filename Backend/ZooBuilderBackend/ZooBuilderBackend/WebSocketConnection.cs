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
            int counter = 0;
            var disconnectedClients = new List<TcpClient>();
            while (isActive)
            {
                var message = $"CALL/Print:Hello, I called you!:{counter}";
                var bytes = Encoding.UTF8.GetBytes(message);
                foreach (var client in connections)
                {
                    try
                    {
                        client.Client.Send(bytes);
                    }
                    catch (Exception ex)
                    {
                        if (client.Connected == false)
                        {
                            disconnectedClients.Add(client);
                        }
                    }
                }
                Thread.Sleep(1000);
                counter++;
            }

            foreach (var client in disconnectedClients)
            {
                connections.Remove(client);
            }
        }
    }
}