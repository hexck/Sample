using Sample.Server.Core.Network;

namespace Sample.Server.Core.Packet.Inbound
{
    public abstract class InboundPacket
    {
        public abstract int Id { get; }

        public abstract string Execute(Client client);
    }
}
