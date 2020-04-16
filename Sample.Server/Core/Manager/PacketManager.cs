using Sample.Server.Core.Handler.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Server.Core.Packet.Inbound.Impl;
using Sample.Server.Core.Network;

namespace Sample.Server.Core.Manager
{
    public class PacketManager
    {
        public List<Packet.Inbound.InboundPacket> Inbound;

        public PacketManager()
        {
            Inbound = new List<Packet.Inbound.InboundPacket>
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
