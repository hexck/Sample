using Sample.Server.Core.Network;
using Sample.Server.Core.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Server.Core.Packet.Outbound.Impl
{
    public class O01StatusLicenseResponse : OutboundPacket
    {
        public override int Id => 0x01;

        public override string Build(Client client)
        {
            return $"{Id.ToString()}|{AesOperation.Encrypt(client, Engine.Instance.LicenseManager.State(client.DecryptedSection(1)).ToString())}";
        }
    }
}
