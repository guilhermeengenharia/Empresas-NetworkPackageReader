﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPRClient.Monitoramento
{
    public interface IDevice
    {
        PcapDotNet.Core.PacketDevice GerarPacketDevice();
    }
}
