
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SharedNetwork;
using UnityEditor.PackageManager;
using UnityEngine;

public class ConnectionHandler : MonoBehaviour
{
    public static ConnectionHandler Instance { get; private set; }

    public bool Connected => Instance._client is { Connected: true };
    public Action<string> ZooNameUpdated;
    
    [SerializeField] private string ip;
    [SerializeField] private int port;
    
    private TcpClient _client;
    
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _client = new TcpClient();
        _client.ConnectAsync(IPAddress.Parse(ip), port);
        StartCoroutine(ReceiveMessages(_client));
    }

    private void OnDisable()
    {
        _client.Dispose();
    }

    IEnumerator ReceiveMessages(TcpClient client)
    {
        yield return new WaitUntil(() => client.Connected);
        Debug.Log("Connected");
        Login();
        while (client.Connected)
        {
            var bytes = new byte[1024];
            var task = client.Client.BeginReceive(bytes, 0, 1024, SocketFlags.None, result =>
            {
                if (result.IsCompleted)
                {
                    Debug.Log(Encoding.UTF8.GetString(bytes));
                    NetworkUtils.ReadMessage(this, bytes);
                }
            }, null);
            yield return new WaitUntil(() => task.IsCompleted);
        }
        Debug.Log("Disconnected");
    }

    private void BuyBuilding(int id)
    {
        string message = MessageBuilder.Call("BuyBuilding").AddParameter(SystemInfo.deviceUniqueIdentifier, id).Build();
        NetworkUtils.TrySendAsync(_client.Client, message);
    }

    private void Login()
    {
        string message = MessageBuilder.Call("Login").AddParameter(SystemInfo.deviceUniqueIdentifier).Build();
        NetworkUtils.TrySendAsync(_client.Client, message);
    }

    private void Print(string message, int counter)
    {
        Debug.Log(message + " | x" + counter);
    }

    private void SetZooName(string zooName)
    {
        ZooNameUpdated?.Invoke(zooName);
    }
}
