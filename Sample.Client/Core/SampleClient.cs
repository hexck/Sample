using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Sample.Client.Core.Util;

namespace Sample.Client.Core
{
    public class SampleClient
    {
        private string _aesKey = "";

        private bool _authenticated;

        public SampleClient(string ip, int port)
        {
            Ip = ip;
            Port = port;
        }

        public Socket Sock { get; private set; }

        public string Ip { get; }

        public int Port { get; }

        public SampleClient Connect()
        {
            try
            {
                Dns.GetHostEntry(Dns.GetHostName());
                var ipAddress = IPAddress.Parse(Ip);

                var remoteEp = new IPEndPoint(ipAddress, Port);

                Sock = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                Sock.Connect(remoteEp);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return this;
        }

        public bool Authenticate()
        {
            try
            {
                Send("0");
                var s0 = Receive().GetString();
                var pubKey = Convert.FromBase64String(s0.Split('|')[1]).GetString();
                var token = s0.Split('|')[2];
                _aesKey = Generator.RandomString(16);
                var encToken = EncryptionUtil.RSAEncrypt(token.GetBytes(), pubKey);
                var encAeskey = EncryptionUtil.RSAEncrypt(_aesKey.GetBytes(), pubKey);
                Send($"1|{encToken}|{encAeskey}");
                _authenticated = true;
                return true;
            }
            catch
            {
            }

            return false;
        }

        public LicenseState State(string key)
        {
            if (!_authenticated)
                throw new Exception("Not authenticated.");

            var data = SendAndReceive($"2|{key.AESEncrypt(_aesKey)}");
            return (LicenseState) Convert.ToInt32(data[1].AESDecrypt(_aesKey));
        }

        // checks if key is connected with hwid
        public bool Valid(string key)
        {
            if (!_authenticated)
                throw new Exception("Not authenticated.");

            var data = SendAndReceive($"3|{key.AESEncrypt(_aesKey)}|{HardwareId.GetHwid().AESEncrypt(_aesKey)}");
            return bool.Parse(data[1].AESDecrypt(_aesKey));
        }

        public RequestState Register(string key)
        {
            if (!_authenticated)
                throw new Exception("Not authenticated.");

            var data = SendAndReceive($"4|{key.AESEncrypt(_aesKey)}|{HardwareId.GetHwid().AESEncrypt(_aesKey)}");
            return (RequestState) int.Parse(data[1].AESDecrypt(_aesKey));
        }

        public bool Whitelisted()
        {
            if (!_authenticated)
                throw new Exception("Not authenticated.");

            var data = SendAndReceive($"5|{HardwareId.GetHwid().AESEncrypt(_aesKey)}");
            return bool.Parse(data[1].AESDecrypt(_aesKey));
        }


        public void Close()
        {
            // Release the socket.  
            Sock.Shutdown(SocketShutdown.Both);
            Sock.Close();
        }

        private byte[] Receive()
        {
            var buf = new byte[4048];
            byte last = 0;
            while (last != 0x05)
            {
                var bytesRead = Sock.Receive(buf, 0, 2048, SocketFlags.None);

                if (bytesRead > 0)
                {
                    last = 0;
                    for (var i = buf.Length - 1; i > 0; i--)
                    {
                        var b = buf[i];
                        if (b != 0x0 && b != 0xA)
                        {
                            last = b;
                            break;
                        }
                    }
                }
            }

            return buf;
        }

        private void Send(string data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            var byteData = Encoding.ASCII.GetBytes(data + "|");

            // Begin sending the data to the remote device.  
            Sock.Send(byteData);
        }

        private string[] SendAndReceive(string data)
        {
            Send(data);
            return Receive().GetString().Split('|');
        }
    }
}