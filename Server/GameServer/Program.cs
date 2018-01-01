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
            tServerPeer.StartServer(5506, 10);
            Console.ReadKey();
        }
    }
}
