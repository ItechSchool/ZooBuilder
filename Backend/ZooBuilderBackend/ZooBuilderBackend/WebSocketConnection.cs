using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharedNetwork;

namespace ZooBuilderBackend
{
    public class WebSocketConnection
    {
        private List<TcpClient> connections = new List<TcpClient>();

        private bool isActive = true;
        private int pingIntervall = 2000;
        public WebSocketConnection(string ip, int port)
        {
            var server = new TcpListener(IPAddress.Parse(ip), port);
            server.Start();
            Console.WriteLine($"Server started on {ip}:{port}");
            var lifeStatusThread = new Thread(PingClients) { IsBackground = true };
            lifeStatusThread.Start();
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
                var clientThread = new Thread(() => ReceiveMessagesFromClient(client))
                {
                    IsBackground = true
                };
                clientThread.Start();
            }
        }

        private void PingClients()
        {
            while (isActive)
            {
                var disconnectedClients = new List<TcpClient>();
                foreach (var client in connections)
                {
                    try
                    {
                        client.Client.Send(Array.Empty<byte>());
                    }
                    catch
                    {
                        disconnectedClients.Add(client);
                    }
                }

                connections.RemoveAll(client => disconnectedClients.Contains(client));
                Thread.Sleep(pingIntervall);
            }
        }
        
        private void ReceiveMessagesFromClient(TcpClient client)
        {
            while (connections.Contains(client))
            {
                var bytes = new byte[1024];
                try
                {
                    client.Client.Receive(bytes);
                }
                catch
                {
                    break;
                }
                NetworkUtils.ReadMessage(this, bytes, client);
            }
            client.Dispose();
        }

        async Task BroadcastTime()
        {
            int counter = 0;
            var disconnectedClients = new List<TcpClient>();
            while (isActive)
            {
                string message = MessageBuilder.Call("Print").AddParameter("Hello, I called you!", counter).Build();
                foreach (var client in connections)
                {
                    if (await NetworkUtils.TrySendAsync(client.Client, message) == false)
                    {
                        if (client.Connected == false)
                        {
                            disconnectedClients.Add(client);
                        }
                    }
                }
                Thread.Sleep(3000);
                counter++;
            }

            foreach (var client in disconnectedClients)
            {
                connections.Remove(client);
                client.Dispose();
            }
        }

        private void Login(TcpClient client, string deviceId)
        {
            //  do some backend stuff
            //  retrieve data

            SendAccountInfo(client);
        }
        
        private void SendAccountInfo(TcpClient client)
        {
            var message = $"CALL/SetZooName:Zoo #{new Random().Next(10000, 99999)}";
            if (NetworkUtils.TrySend(client.Client, message) == false)
            {
                if (client.Connected == false)
                {
                    connections.Remove(client);
                }
            }
        }
        
        private void BuyBuilding(TcpClient client, string clientId, int buildingId)
        {
            Console.WriteLine($"Client with id: {clientId} bought building {buildingId}");
        }
    }
}