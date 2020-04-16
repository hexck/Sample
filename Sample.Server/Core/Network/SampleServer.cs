using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Sample.Server.Core.Database;
using Sample.Server.Core.Logging;

namespace Sample.Server.Core.Network
{
    public class SampleServer
    {
        public SampleServer()
        {
            Pause = false;
            Listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
            Listener.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), Constants.Port));
        }

        public Socket Listener { get; }

        public bool Cancel { get; set; }

        public bool Pause { get; set; }

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
                new Thread(() => HandleClient(new Client {Sock = sock})).Start();
            }
        }

        private void HandleClient(Client client)
        {
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
                        client.Sock.Close();
                        return;
                    }

                    Receive(client);
                    client.Clear();
                }
            }
            catch
            {
            }
        }

        private void Receive(Client client)
        {
            var bytesRead = client.Sock.Receive(client.Buffer, 0, Client.BufferSize, SocketFlags.None);

            if (bytesRead <= 0) return;

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
                Logger.Log($"{client.IpAddress} -> banned for 30 days", LogType.Critical);
                new MongoCrud().Ban(client.IpAddress, 30);
                client.Sock.Close();
                return;
            }

            if (last == 0x05)
                try
                {
                    var id = Convert.ToInt32(((char) client.Buffer[0]).ToString());
                    Send(client, Engine.Instance.PacketManager.Execute(client, id));
                }
                catch
                {
                    Logger.Log($"{client.IpAddress} -> something went wrong: closing socket.", LogType.Error);
                    client.Sock.Close();
                }
            else
                Receive(client);
        }

        private void Send(Client client, string data)
        {
            if (data.Length == 0) // no need to send
                return;

            client.Sock.Send(Encoding.ASCII.GetBytes(data + "|"));
        }
    }
}