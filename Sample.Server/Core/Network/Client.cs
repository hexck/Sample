using System.Net;
using System.Net.Sockets;
using System.Text;
using Sample.Server.Core.Util;

namespace Sample.Server.Core.Network
{
    public class Client
    {
        // Size of receive buffer.  
        public const int BufferSize = 1024;

        public byte[] Buffer = new byte[BufferSize];

        // Client  socket.  
        public Socket Sock = null;

        public string VerifyToken { get; set; }

        public string SecretKey { get; set; }

        public string Data => Encoding.UTF8.GetString(Buffer);

        public string IData => Data.Substring(2, Data.Length - 4);

        public string[] Sections => Data.Split('|');

        public string IpAddress => ((IPEndPoint) Sock.RemoteEndPoint).Address.ToString();

        public string DecryptedSection(int x)
        {
            return SecretKey == "" ? "" : AesOperation.Decrypt(this, Sections[x]);
        }

        public void Clear()
        {
            Buffer = new byte[BufferSize];
        }
    }
}