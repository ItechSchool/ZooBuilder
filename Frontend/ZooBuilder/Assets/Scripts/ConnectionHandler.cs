
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEngine;

public class ConnectionHandler : MonoBehaviour
{
    [SerializeField] private string ip;
    [SerializeField] private int port;
    void Start()
    {
        var client = new TcpClient();
        client.Connect(IPAddress.Parse(ip), port);
        Debug.Log("Connected");
        StartCoroutine(ReceiveMessages(client));
    }

    IEnumerator ReceiveMessages(TcpClient client)
    {
        while (client.Connected)
        {
            var bytes = new byte[1024];
            //var segment = new ArraySegment<byte>(bytes, 0, 1024);
            var task = client.Client.BeginReceive(bytes, 0, 1024, SocketFlags.None, result =>
            {
                if (result.IsCompleted)
                {
                    ReadMessage(Encoding.UTF8.GetString(bytes));
                }
            }, null);
            yield return new WaitUntil(() => task.IsCompleted);
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
                var method = typeof(ConnectionHandler).GetMethod(methodName);
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
                Debug.Log(message);
                break;
        }
    }

    public void Print(string message, int counter)
    {
        Debug.Log(message + " | x" + counter);
    }
}
