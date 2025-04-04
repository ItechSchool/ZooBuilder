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
            if (message.Length < 3) return false;
            byte[] bytes = Encoding.UTF8.GetBytes(message);
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
            if (message.Length < 3) return false;
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

        public static void ReadBuffer(List<byte> buffer, byte[] newBytes, Queue<string> messageQueue)
        {
            string message = Encoding.UTF8.GetString(newBytes);
            if (buffer.Count > 0)
            {
                message = Encoding.UTF8.GetString(buffer.ToArray()) + message;
            }
            if (message.Contains("\\s") == false)
            {
                buffer.AddRange(newBytes);
                return;
            }
            buffer.Clear();
            while (message.Contains("\\s"))
            {
                string request = message.Split("\\s")[0];
                messageQueue.Enqueue(request);
                message = message[(request.Length + 2)..];
            }
            buffer.AddRange(Encoding.UTF8.GetBytes(message));
        }

        public static void ReadMessage<T>(T caller, byte[] bytes, params object[] additionalArgs)
        {
            string message = RemoveNonPrintableCharacters(Encoding.UTF8.GetString(bytes));
            ReadMessage(caller, message, additionalArgs);
        }

        public static void ReadMessage<T>(T caller, string message, params object[] additionalArgs)
        {
            message = RemoveNonPrintableCharacters(message);
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

        public static string RemoveNonPrintableCharacters(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            StringBuilder builder = new StringBuilder();

            foreach (char c in input)
            {
                // Keep only characters that are not control characters
                // Alternatively: (c >= 32 && c <= 126) for printable ASCII
                if (!char.IsControl(c))
                {
                    builder.Append(c);
                }
            }

            return builder.ToString();
        }
    }
}