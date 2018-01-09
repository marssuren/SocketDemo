using System;
using Server;


namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerPeer tServerPeer = new ServerPeer();
            tServerPeer.SetApplication(new NetMsgCenter());     //指定所关联的应用

            tServerPeer.StartServer(5056, 10);
            Console.ReadKey();
        }
    }
}
