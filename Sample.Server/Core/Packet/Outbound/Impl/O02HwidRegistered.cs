using Sample.Server.Core.Network;
using Sample.Server.Core.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Server.Core.Packet.Outbound.Impl
{
    public class O02HwidRegistered : OutboundPacket
    {
        public override int Id => 0x02;

        public override string Build(Client client)
        {
            return $"{Id.ToString()}|{AesOperation.Encrypt(client, Engine.Instance.LicenseManager.Valid(client.DecryptedSection(1), client.DecryptedSection(2)).ToString())}";
        }
    }
}
