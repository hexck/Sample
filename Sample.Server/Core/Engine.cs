using Sample.Server.Core.Logging;
using Sample.Server.Core.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Sample.Server.Core.Network;

namespace Sample.Server.Core
{
    public class Engine
    {
        public static Engine Instance;

        public SampleServer SampleServer { get; }

        public EncryptionManager EncryptionManager { get; }

        public PacketManager PacketManager { get; }

        public LicenseManager LicenseManager { get; }

        public Engine()
        {
            Instance = this;
            EncryptionManager = new EncryptionManager();
            LicenseManager = new LicenseManager();
            PacketManager = new PacketManager();
            SampleServer = new SampleServer();
            SampleServer.Start();
        }
    }
}
