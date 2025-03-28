
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
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
                    Debug.Log(Encoding.UTF8.GetString(bytes));
                }
            }, null);
            yield return new WaitUntil(() => task.IsCompleted);
        }
    }

    private void Print(string message)
    {
        Debug.Log(message);
    }
}
