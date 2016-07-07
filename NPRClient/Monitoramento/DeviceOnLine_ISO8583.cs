using PcapDotNet.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPRClient.Monitoramento
{
    public class DeviceOnLine_ISO8583 : IDevice
    {
        public PcapDotNet.Core.PacketDevice GerarPacketDevice()
        {
            PacketDevice selectedDevice = null;

            //Carrega todas as interfaces de rede disponiveis na maquina
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;

            if (allDevices.Count == 0)
            {
                Console.WriteLine("Não foi possivel ler as interfaces de rede! Verifique se o WinPcap está instalado.");
                return null;
            }

            // Mostra a lista de interfaces de rede disponiveis
            for (int i = 0; i != allDevices.Count; ++i)
            {
                LivePacketDevice device = allDevices[i];
                Console.Write((i + 1) + ". " + device.Name);
                if (device.Description != null)
                    Console.WriteLine(" (" + device.Description + ")");
                else
                    Console.WriteLine(" (Sem descrição informada)");
            }

            int deviceIndex = 0;
            string NetWorkAdapterSelecionado = ConfigurationManager.AppSettings["NetworkAdapter"].ToString();

            //Carrega a interface escolhida
            for (deviceIndex = 0; deviceIndex < allDevices.Count; deviceIndex++)
            {
                if (allDevices[deviceIndex].Description.IndexOf(NetWorkAdapterSelecionado) > 0)
                {
                    selectedDevice = allDevices[deviceIndex];
                }
            }

            return selectedDevice;

        }

    }
}
