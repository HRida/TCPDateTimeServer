﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace TCPDateTimeServer
{
    class DateServer
    {
        private TcpListener myListener;
        private int port = 4554;

        public DateServer()
        {
            try
            {
                myListener = new TcpListener(4554);
                myListener.Start();
                Console.WriteLine("Server Ready -Listening for new Connections ");
                Thread th = new Thread(new ThreadStart(StartListen));
                th.Start();

            }
            catch (Exception e)
            {
                Console.WriteLine("An Exception Occurred while Listening :"  + e.ToString());
            }
        }

        public static void Main(String[] argv)
        {
            DateServer dts = new DateServer();
        }
        public void StartListen()
        {
            while (true)
            {
                Socket mySocket = myListener.AcceptSocket();
                if (mySocket.Connected)
                {
                    Console.WriteLine("Client Connected!!");
                    Byte[] receive = new Byte[64];
                    int i = mySocket.Receive(receive, receive.Length, 0);
                    char[] unwanted = { ' ', ' ', ' ' };
                    string rece = System.Text.Encoding.ASCII.GetString(receive);
                    Console.WriteLine(rece.TrimEnd(unwanted));
                    DateTime now = DateTime.Now;
                    String strDateLine = "Server: The Date/Time Now is: "
                                + now.ToShortDateString()
                      + " " + now.ToShortTimeString();
                    Byte[] byteDateLine =
                           System.Text.Encoding.ASCII.GetBytes(strDateLine.ToCharArray());
                    mySocket.Send(byteDateLine, byteDateLine.Length, 0);
                }
            }
        }
    }
}