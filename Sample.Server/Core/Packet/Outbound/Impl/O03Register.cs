using Sample.Server.Core.Network;
using Sample.Server.Core.Util;
using System;
using System.Collections.Generic;
using System.Text;
using Sample.Server.Core.Logging;

namespace Sample.Server.Core.Packet.Outbound.Impl
{
    public class O03Register : OutboundPacket
    {
        public override int Id => 0x01;

        public override string Build(Client client)
        {
            var state = Engine.Instance.LicenseManager.State(client.DecryptedSection(1));
            if (state == (int)LicenseState.Occupied || state == (int)LicenseState.Invalid)
                return $"{Id.ToString()}|{AesOperation.Encrypt(client, ((int)RequestState.Fail).ToString())}|{AesOperation.Encrypt(client, state.ToString())}";
            Logger.Log($"{client.IpAddress} -> bound license {client.DecryptedSection(1)} with hwid {client.DecryptedSection(2)}");
            Engine.Instance.LicenseManager.Register(client.DecryptedSection(1), client.DecryptedSection(2));
            return $"{Id.ToString()}|{AesOperation.Encrypt(client, ((int)RequestState.Success).ToString())}|{AesOperation.Encrypt(client, state.ToString())}";
        }
    }
}
