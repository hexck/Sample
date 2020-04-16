using Sample.Server.Core.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Sample.Server.Core.Util;
using Sample.Server.Core.Logging;
using Sample.Server.Core.Database;
using Sample.Server.Core.Database.Models;

namespace Sample.Server.Core.Handler.Impl
{
    public class I01EncryptionResponse : Packet.Inbound.InboundPacket
    {
        public override int Id => 0x01;

        // structure: 0x31, 0x7C, ..... encrypted
        public override string Execute(Client client)
        {
            var token = Engine.Instance.EncryptionManager.Decrypt(client.Sections[1]).GetString();
            client.SecretKey = Engine.Instance.EncryptionManager.Decrypt(client.Sections[2]).GetString();

            if (token != client.VerifyToken) // token is different, so gg!
            {
                Logger.Log($"{client.IpAddress} -> failed to return correct token, ip blocked for 30 days", LogType.Critical);
                new MongoCrud().Ban(client.IpAddress, 30);
                client.Sock.Close();
            }
            else
                Logger.Log($"[+] {client.IpAddress} -> authenticated");
                

            return "";
        }
    }
}
