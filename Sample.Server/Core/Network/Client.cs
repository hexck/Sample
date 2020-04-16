using Sample.Server.Core.Util;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Sample.Server.Core.Network
{
    public class Client
    {
        // Client  socket.  
        public Socket Sock = null;
        
        // Size of receive buffer.  
        public const int BufferSize = 1024;

        public string VerifyToken { get; set; }

        public string SecretKey { get; set; }

        public byte[] Buffer = new byte[BufferSize];

        public string Data => Encoding.UTF8.GetString(Buffer);

        public string IData => Data.Substring(2, Data.Length - 4);

        public string[] Sections => Data.Split('|');

        public string IpAddress => ((IPEndPoint)Sock.RemoteEndPoint).Address.ToString();

        public string DecryptedSection(int x)
        {
            if (SecretKey == "")
                return "";

            return AesOperation.Decrypt(this, Sections[x]);
        }
        public void Clear()
        {
            Buffer = new byte[BufferSize];
        }
    }
}
