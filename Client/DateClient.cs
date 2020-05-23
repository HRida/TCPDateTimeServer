using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Client
{
    public class DateClient
    {
        private TcpClient tcpc;
        private string name;
        private int port = 4554;
        private bool readData = false;

        public DateClient(string name)
        {
        tryagain:
            this.name = name;
            try
            {

                tcpc = new TcpClient("localhost",4554);
                NetworkStream nts = tcpc.GetStream();
                if (nts.CanWrite)
                {
                    string sender = "Hi Server I am " + name;
                    Byte[] sends = System.Text.Encoding.ASCII.GetBytes(sender.ToCharArray());
                    nts.Flush();
                }
                while (!readData && nts.CanRead)
                {
                    if (nts.DataAvailable)
                    {
                        byte[] rcd = new byte[128];
                        int i = nts.Read(rcd, 0, 128);
                        string ree = System.Text.Encoding.ASCII.GetString(rcd);
                        char[] unwanted = { ' ', ' ', ' ' };
                        Console.WriteLine(ree.TrimEnd(unwanted));
                        readData = true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not Connect to server because " + e.ToString());

                Console.Write("Do you want to try Again? [y/n]: ");
                string check = Console.ReadLine();
                if (check[0] == 'y' || check[0] == 'Y')
                    goto tryagain;
            }
        }

        public static void Main(string[] argv)
        {
            if (argv.Length <= 0)
            {
                Console.WriteLine("Usage: DataClient <yourname>");
                Console.Write("Would You like to enter your name now [y/n] ?");
                string check = Console.ReadLine();
                if (check[0] == 'y' || check[0] == 'Y')
                {
                    Console.Write("Please enter you name :");
                    string newname = Console.ReadLine();
                    DateClient dc = new DateClient(newname);
                    Console.WriteLine("Disconnected!!");
                    Console.ReadLine();
                }
            }
            else
            {
                DateClient dc = new DateClient(argv[0]);
                Console.WriteLine("Disconnected!!");
                Console.ReadLine();
            }
        }
    }
}