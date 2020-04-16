using Sample.Server.Core.Network;
using Sample.Server.Core.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Server.Core.Packet.Outbound.Impl
{
    public class O04Whitelisted : OutboundPacket
    {
        public override int Id => 0x04;

        public override string Build(Client client)
        {
            return $"{Id.ToString()}|{AesOperation.Encrypt(client, Engine.Instance.LicenseManager.Whitelisted(client.DecryptedSection(1)).ToString())}";
        }
    }
}
