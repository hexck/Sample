using Sample.Server.Core.Logging;
using Sample.Server.Core.Network;
using Sample.Server.Core.Packet.Outbound.Impl;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Server.Core.Packet.Inbound.Impl
{
    public class I02StatusLicenseRequest : InboundPacket
    {
        public override int Id => 0x02;

        // structure: 0x31, 0x7C, ..... encrypted
        public override string Execute(Client client)
        {
            return new O01StatusLicenseResponse().Build(client);
        }
    }
}
