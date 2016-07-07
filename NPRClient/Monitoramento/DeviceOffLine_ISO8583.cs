using PcapDotNet.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPRClient.Monitoramento
{
    public class DeviceOffLine_ISO8583 : IDevice
    {
        public PcapDotNet.Core.PacketDevice GerarPacketDevice()
        {
            PacketDevice selectedDevice;

            selectedDevice = new OfflinePacketDevice(ConfigurationManager.AppSettings["ReaderFromFileDirectory"].ToString());

            return selectedDevice;
        }
    }
}
