using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server;


namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerPeer tServerPeer = new ServerPeer();
            tServerPeer.SetApplication(new NetMsgCenter());     //指定所关联的应用

            tServerPeer.StartServer(6666, 10);
            Console.ReadKey();
        }
    }
}
