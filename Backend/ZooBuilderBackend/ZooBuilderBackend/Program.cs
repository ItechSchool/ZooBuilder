using System;
using SharedNetwork;

namespace ZooBuilderBackend
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var connection = new WebSocketConnection("127.0.0.1", 1200);
            Console.ReadKey(true);
        }
    }
}