using Sample.Server.Core.Network;

namespace Sample.Server.Core.Packet.Outbound
{
    public abstract class OutboundPacket
    {
        public abstract int Id { get; }

        public abstract string Build(Client client);
    }
}
