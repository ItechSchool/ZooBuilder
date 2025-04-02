using System.Net;
using System.Net.Sockets;
using System.Reflection;
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
                SendAccountInfo(client);
                Console.WriteLine($"Client connected");
            }
        }

        protected bool TrySendMessageToClient(TcpClient client, string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            try
            {
                client.Client.Send(bytes);
                return true;
            }
            catch (Exception ex)
            {
                return false;
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
                Thread.Sleep(10000);
                counter++;
            }

            foreach (var client in disconnectedClients)
            {
                connections.Remove(client);
                client.Dispose();
            }
        }
        
        private void SendAccountInfo(TcpClient client)
        {
            var message = $"CALL/SetZooName:Zoo #{new Random().Next(10000, 99999)}";
            if (TrySendMessageToClient(client, message) == false)
            {
                if (client.Connected == false)
                {
                    connections.Remove(client);
                }
            }
        }
        
        
        private void ReadMessage(string message)
        {
            if (message.Length < 3) return;
            string command = message.Substring(0, message.IndexOf("/"));
            string[] arguments = message.Substring(command.Length + 1, message.Length - command.Length - 1).Split(":");
            switch (command)
            {
                case "CALL":
                    string methodName = arguments[0];
                    var method = typeof(WebSocketConnection).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
                    if (method == null)
                        break;
                    var expectedArguments = method.GetParameters();
                    var args = new List<object>();
                    for (var i = 0; i < expectedArguments.Length; i++)
                    {
                        var expectedArgument = i < expectedArguments.Length ? expectedArguments[i] : null;
                        string argument = arguments[i + 1];
                        try
                        {
                            args.Add(Convert.ChangeType(argument, expectedArgument.ParameterType));
                        }
                        catch
                        {
                            args.Add(null);
                        }
                    }
                    method.Invoke(this, args.ToArray());
                    break;
                default:
                    break;
            }
        }
    }
}