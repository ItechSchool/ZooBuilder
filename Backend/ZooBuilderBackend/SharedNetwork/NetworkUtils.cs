using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharedNetwork
{
    public static class NetworkUtils
    {
        public static bool TrySend(Socket socket, string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            try
            {
                socket.Send(bytes);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> TrySendAsync(Socket socket, string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            try
            {
                await socket.SendAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void ReadMessage<T>(T caller, byte[] bytes, params object[] additionalArgs)
        {
            string message = Encoding.UTF8.GetString(bytes);
            ReadMessage(caller, message, additionalArgs);
        }

        public static void ReadMessage<T>(T caller, string message, params object[] additionalArgs)
        {
            if (message.Length < 3 || message.Contains("/") == false) return;
            string command = message.Substring(0, message.IndexOf("/"));
            string[] arguments = message.Substring(command.Length + 1, message.Length - command.Length - 1).Split(":");
            switch (command)
            {
                case "CALL":
                    string methodName = arguments[0];
                    var method = typeof(T).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
                    if (method == null)
                        break;
                    var expectedArguments = method.GetParameters();
                    var args = new List<object>(additionalArgs);
                    for (int i = additionalArgs.Length; i < expectedArguments.Length; i++)
                    {
                        var expectedArgument = i < expectedArguments.Length ? expectedArguments[i] : null;
                        string argument = arguments[i - additionalArgs.Length + 1];
                        try
                        {
                            if (expectedArgument.ParameterType.GetInterfaces().Contains(typeof(IStringSerializable)))
                            {
                                Console.WriteLine("Serializable object found");
                                var castedObject = (IStringSerializable)Activator.CreateInstance(expectedArgument.ParameterType);
                                castedObject.FromString(argument);
                                args.Add(castedObject);
                            }
                            else
                            {
                                args.Add(Convert.ChangeType(argument, expectedArgument.ParameterType));
                            }
                        }
                        catch
                        {
                            args.Add(null);
                        }
                    }
                    method.Invoke(caller, args.ToArray());
                    break;
                default:
                    break;
            }
        }
    }
}