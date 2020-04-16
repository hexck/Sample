using Sample.Server.Core.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Server.Core.Packet.Outbound.Impl
{
    public class O00EncryptionRequest : OutboundPacket
    {
        public override int Id => 0x00;

        public override string Build(Client client)
        {
            client.VerifyToken = Guid.NewGuid().GetHashCode().ToString(); // generate random secret 
            var s = Convert.ToBase64String( Encoding.UTF8.GetBytes( Engine.Instance.EncryptionManager.PublicKey ) );
            
            return $"{Id.ToString()}|{s}|{client.VerifyToken}";
        }
    }
}
