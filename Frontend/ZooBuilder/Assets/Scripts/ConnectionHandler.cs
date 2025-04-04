
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

    public bool Connected => _connected;
    public Action<BuildingDto> BuildingAdded;
    public Action<AnimalDto> AnimalAdded;
    public Action<GridPlacementDto> GridPlacementAdded;
    public Action<ZooDto> ZooInfoUpdated;

    public Action<string> ThrowError;
    
    [SerializeField] private string ip;
    [SerializeField] private int port;
    
    [SerializeField] private int healthCheckInterval;
    [SerializeField] private float _retryConnectTime;
    
    private TcpClient _client;
    private readonly Queue<string> _messageQueue = new Queue<string>();
    private readonly List<byte> _buffer = new List<byte>();

    private bool _connected;
    private bool _tryingToConnect;

    private float _timeToConnect;
    
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ConnectToServer();
    }
    
    private void Update()
    {
        if (_tryingToConnect && _connected == false)
        {
            _timeToConnect += Time.deltaTime;
            if (_timeToConnect >= _retryConnectTime)
            {
                ConnectToServer();
            }
        }
    }

    private void ConnectToServer()
    {
        StopAllCoroutines();
        _client = new TcpClient();
        _client.ConnectAsync(IPAddress.Parse(ip), port);
        _tryingToConnect = true;
        _timeToConnect = 0f;
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
        yield return new WaitUntil(() => _connected);
        while (_connected)
        {
            yield return null;
            if (_messageQueue.Count == 0) continue;
            
            var message = _messageQueue.Dequeue();
            Debug.Log(message);
            NetworkUtils.ReadMessage(this, message);
        }
    }
    
    private IEnumerator ReceiveMessages()
    {
        yield return new WaitUntil(() => _client.Connected);
        _connected = true;
        Debug.Log("Connected");
        StartCoroutine(PingServer());
        StartCoroutine(AwaitLogin());
        while (_connected)
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

    private IEnumerator AwaitLogin()
    {
        yield return new WaitUntil(() => _connected);
        yield return null;
        Login();
    }
    
    private IEnumerator PingServer()
    {
        Debug.Log("Pinging server");
        while (_connected)
        {
            try
            {
                _client.Client.Send(Array.Empty<byte>());
            }
            catch
            {
                break;
            }

            yield return new WaitForSeconds(healthCheckInterval / 1000f);
        }
        Debug.LogWarning("Disconnected from server");
        _connected = false;
        _timeToConnect = 0f;
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
