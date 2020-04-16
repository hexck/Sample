using Sample.Server.Core.Network;
using Sample.Server.Core.Packet.Outbound.Impl;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Server.Core.Packet.Inbound.Impl
{
    public class I04Register : InboundPacket
    {
        public override int Id => 0x04;

        public override string Execute(Client client)
        {
            return new O03Register().Build(client);
        }
    }
}
