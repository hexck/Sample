using Sample.Server.Core.Network;
using Sample.Server.Core.Packet.Outbound.Impl;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Server.Core.Packet.Inbound.Impl
{
    public class I00Handshake : InboundPacket
    {
        public override int Id => 0x00;

        public override string Execute(Client client)
        {
            return new O00EncryptionRequest().Build(client);
        }
    }
}
