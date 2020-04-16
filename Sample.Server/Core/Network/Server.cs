using Sample.Server.Core.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Sample.Server.Core.Database;

namespace Sample.Server.Core.Network
{
    public class Server
    {
        public Socket Listener { get; private set; }

        public bool Cancel { get; set; }

        public bool Pause { get; set; }

        public Server()
        {
            Pause = false;
            Listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
            Listener.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), Constants.Port));
        }

        public void Start()
        {
            new Thread(() => Task()).Start();
        }

        public void Stop()
        {
            Cancel = true;
        }

        private void Task()
        {
            Listener.Listen(100);

            while (!Cancel)
            {
                if (Pause)
                {
                    Thread.Sleep(2000);
                    continue;
                }
                var sock = Listener.Accept();
                new Thread(() => HandleClient(sock)).Start();
            }
        }

        private void HandleClient(Socket sock)
        {
            var client = new Client();
            client.Sock = sock;
            if (Engine.Instance.LicenseManager.IsBanned(client.IpAddress))
            {
                client.Sock.Close();
                return;
            }

            try
            {
                while (!Cancel)
                {
                    if (Pause)
                    {
                        Thread.Sleep(2000);
                        continue;
                    }
                    Receive(client);
                    client.Clear();
                }
            } catch {    }
        }

        public void Receive(Client client)
        {
            var bytesRead = client.Sock.Receive(client.Buffer, 0, Client.BufferSize, SocketFlags.None);

            if (bytesRead > 0)
            {
                byte last = 0;
                for (var i = client.Buffer.Length - 1; i > 0; i--)
                {
                    var b = client.Buffer[i];
                    if (b != 0x0 && b != 0xA)
                    {
                        last = b;
                        break;
                    }
                }

                if (!client.Data.Contains('|')) // packets always contain an '|'
                {
                    Logger.Log($"{client.IpAddress} -> ip has been blocked for 30 days", LogType.Critical);
                    new MongoCrud().Ban(client.IpAddress, 30);
                    client.Sock.Close();
                    return;
                }

                if (last == 0x05)
                {
                    try
                    {
                        var id = Convert.ToInt32(((char)client.Buffer[0]).ToString());
                        Console.WriteLine();
                        Send(client, Engine.Instance.PacketManager.Execute(client, id));
                    }
                    catch
                    {
                        Logger.Log($"{client.IpAddress} -> something went wrong: closing socket.", LogType.Error);
                        client.Sock.Close();
                    }
                }
                else
                {
                    Receive(client);
                }
            }

        }

        private void Send(Client client, string data)
        {
            if (data.Length == 0) // no need to send
                return;

            client.Sock.Send(Encoding.ASCII.GetBytes(data + "|"));
        }
    }
}
