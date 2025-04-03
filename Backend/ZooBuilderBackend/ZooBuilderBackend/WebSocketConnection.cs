using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharedNetwork;
using SharedNetwork.Dtos;
using ZooBuilderBackend.Data;
using ZooBuilderBackend.Services;

namespace ZooBuilderBackend
{
    public class WebSocketConnection
    {
        private List<TcpClient> connections = new();
        private static readonly ApplicationDbContext Db = new();
        private readonly StartUpService _startUpService = new(Db);
        private readonly PlayerService _playerService = new(Db);

        private bool isActive = true;
        private int pingIntervall = 2000;

        private List<byte> _buffer = [];
        private Queue<string> _messageQueue = new();
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
                    NetworkUtils.ReadBuffer(_buffer, bytes, _messageQueue);
                }
                catch
                {
                    break;
                }

                while (_messageQueue.Count > 0)
                {
                    NetworkUtils.ReadMessage(this, _messageQueue.Dequeue(), client);
                }
            }
            client.Dispose();
        }

        async Task BroadcastTime()
        {
            return;
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
            try
            {
                _playerService.Login(deviceId);
                var startUpDataDto = _startUpService.LoadStartUpData(deviceId);
                SendStartUpData(client, startUpDataDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                SendException(client, ex.Message.Replace(":", ";"));
            }
        }

        private void SendException(TcpClient client, string message)
        {
            string request = MessageBuilder.Call("Exception").AddParameter(message).Build();
            NetworkUtils.TrySend(client.Client, request);
        }

        private void SendStartUpData(TcpClient client, StartUpDataDto startUpDataDto)
        {
            foreach (var building in startUpDataDto.Buildings)
            {
                var buildingMessage = MessageBuilder.Call("LoadBuilding").AddParameter(building).Build();
                NetworkUtils.TrySend(client.Client, buildingMessage);
            }

            foreach (var animal in startUpDataDto.Animals)
            {
                var animalMessage = MessageBuilder.Call("LoadAnimal").AddParameter(animal).Build();
                NetworkUtils.TrySend(client.Client, animalMessage);
            }

            foreach (var gridPlacement in startUpDataDto.GridPlacements)
            {
                var gridPlacementMessage = MessageBuilder.Call("LoadGridPlacement").AddParameter(gridPlacement).Build();
                NetworkUtils.TrySend(client.Client, gridPlacementMessage);
            }

            var zooMessage = MessageBuilder.Call("LoadZoo").AddParameter(startUpDataDto.Zoo).Build();
            NetworkUtils.TrySend(client.Client, zooMessage);
            Console.WriteLine("Finished sending data");
        }

        private void BuyBuilding(TcpClient client, string clientId, int buildingId)
        {
            Console.WriteLine($"Client with id: {clientId} bought building {buildingId}");
        }
    }
}