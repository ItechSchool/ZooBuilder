
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SharedNetwork;
using SharedNetwork.Dtos;
using UnityEngine;

public class ConnectionHandler : MonoBehaviour
{
    public static ConnectionHandler Instance { get; private set; }

    public bool Connected => Instance._client is { Connected: true };
    public Action<BuildingDto> BuildingAdded;
    public Action<AnimalDto> AnimalAdded;
    public Action<GridPlacementDto> GridPlacementAdded;
    public Action<ZooDto> ZooInfoUpdated;

    public Action<string> ThrowError;
    
    [SerializeField] private string ip;
    [SerializeField] private int port;
    
    private TcpClient _client;
    private readonly Queue<string> _messageQueue = new Queue<string>();
    private List<byte> _buffer = new List<byte>();
    
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _client = new TcpClient();
        _client.ConnectAsync(IPAddress.Parse(ip), port);
        StartCoroutine(ReceiveMessages());
        StartCoroutine(ReadMessages());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        _client.Dispose();
    }

    private IEnumerator ReadMessages()
    {
        while (_client.Connected)
        {
            yield return null;
            if (_messageQueue.Count == 0) continue;
            
            NetworkUtils.ReadMessage(this, _messageQueue.Dequeue());
        }
    }
    
    private IEnumerator ReceiveMessages()
    {
        yield return new WaitUntil(() => _client.Connected);
        Debug.Log("Connected");
        Login();
        while (_client.Connected)
        {
            var bytes = new byte[1024];
            var task = _client.Client.BeginReceive(bytes, 0, 1024, SocketFlags.None, result =>
            {
                if (result.IsCompleted)
                {
                    NetworkUtils.ReadBuffer(_buffer, bytes, _messageQueue);
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
    private void LoadBuilding(BuildingDto building)
    {
        BuildingAdded?.Invoke(building);
    }

    private void LoadAnimal(AnimalDto animal)
    {
        AnimalAdded?.Invoke(animal);
    }

    private void LoadGridPlacement(GridPlacementDto placement)
    {
        GridPlacementAdded?.Invoke(placement);
    }

    private void LoadZoo(ZooDto zoo)
    {
        Debug.Log(zoo);
        ZooInfoUpdated?.Invoke(zoo);
    }

    private void Exception(string message)
    {
        ThrowError?.Invoke(message);
    }
}
