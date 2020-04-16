using System.Collections.Generic;
using System.Linq;
using Sample.Server.Core.Handler.Impl;
using Sample.Server.Core.Network;
using Sample.Server.Core.Packet.Inbound;
using Sample.Server.Core.Packet.Inbound.Impl;

namespace Sample.Server.Core.Manager
{
    public class PacketManager
    {
        public List<InboundPacket> Inbound;

        public PacketManager()
        {
            Inbound = new List<InboundPacket>
            {
                new I00Handshake(),
                new I01EncryptionResponse(),
                new I02StatusLicenseRequest(),
                new I03HwidRegistered(),
                new I04Register(),
                new I05Whitelisted()
            };
        }

        public string Execute(Client client, int id)
        {
            var target = Inbound.FirstOrDefault(i => i.Id == id);
            return target is null ? "" : target.Execute(client);
        }
    }
}