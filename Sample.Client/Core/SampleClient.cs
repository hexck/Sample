using Sample.Client.Core.Util;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Sample.Client.Core
{
    public class SampleClient
    {
        public Socket Sock { get; private set; }

        public string Ip { get; }

        public int Port { get; }

        private bool _authenticated;
        private string _aesKey = "";

        public SampleClient(string ip, int port)
        {
            Ip = ip;
            Port = port;
        }

        public SampleClient Connect()
        {
            try
            {
                Dns.GetHostEntry(Dns.GetHostName());
                var ipAddress = IPAddress.Parse(Ip);

                IPEndPoint remoteEp = new IPEndPoint(ipAddress, Port);

                Sock = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                Sock.Connect(remoteEp);
            }
            catch (Exception e) {  Console.WriteLine(e.ToString());  }

            return this;
        }

        public bool Authenticate()
        {
            try {
                Send("0");
                var s0 = Receive().GetString();
                var pubKey = Convert.FromBase64String(s0.Split('|')[1]).GetString();
                _aesKey = Generator.RandomString(16);
                var token = s0.Split('|')[2];
                var encToken = EncryptionUtil.RSAEncrypt(token.GetBytes(), pubKey);
                var encAeskey = EncryptionUtil.RSAEncrypt(Encoding.UTF8.GetBytes(_aesKey), pubKey);
                Send($"1|{encToken}|{encAeskey}");
                _authenticated = true;
                return true;
            }
            catch {   }
            return false;
        }

        public LicenseState State(string key)
        {
            if (!_authenticated)
                throw new Exception("Not authenticated.");

            Send($"2|{EncryptionUtil.AESEncrypt(_aesKey, key)}");
            var s = Receive().GetString().Split('|')[1];
            return (LicenseState) Convert.ToInt32(EncryptionUtil.AESDecrypt(_aesKey, s));
        }

        // checks if key is connected with hwid
        public bool Valid(string key)
        {
            if (!_authenticated)
                throw new Exception("Not authenticated.");

            Send($"3|{key.AESEncrypt(_aesKey)}|{HardwareId.GetHwid().AESEncrypt(_aesKey)}");
            var s = Receive().GetString().Split('|')[1];
            return bool.Parse(s.AESDecrypt(_aesKey));
        }
        
        public bool Whitelisted()
        {
            if (!_authenticated)
                throw new Exception("Not authenticated.");
            
            var hwid = HardwareId.GetHwid();
            Send($"5|{hwid.AESEncrypt(_aesKey)}");
            var s = Receive().GetString().Split('|')[1];
            return bool.Parse(s.AESDecrypt(_aesKey));
        }

        public RequestState Register(string key)
        {
            if (!_authenticated)
                throw new Exception("Not authenticated.");

            Send($"4|{key.AESEncrypt(_aesKey)}|{HardwareId.GetHwid().AESEncrypt(_aesKey)}");
            var s = Receive().GetString().Split('|')[1];
            return (RequestState)int.Parse(s.AESDecrypt(_aesKey));
        }


        public void Close()
        {
            // Release the socket.  
            Sock.Shutdown(SocketShutdown.Both);
            Sock.Close();
        }

        private byte[] Receive()
        {
            byte[] buf = new byte[4048];
            byte last = 0;
            while (last != 0x05)
            {
                int bytesRead = Sock.Receive(buf, 0, 2048, SocketFlags.None);

                if (bytesRead > 0)
                {
                    last = 0;
                    for (int i = buf.Length - 1; i > 0; i--)
                    {
                        byte b = buf[i];
                        if ((b != 0x0) && (b != 0xA))
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
            byte[] byteData = Encoding.ASCII.GetBytes(data+ "|");

            // Begin sending the data to the remote device.  
            Sock.Send(byteData);
        }
    }
}
