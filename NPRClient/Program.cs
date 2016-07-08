using NPRClient.ENUN;
using NPRClient.Monitoramento;
using PcapDotNet.Core;
using System;
using System.Configuration;


namespace NPRClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("INICIANDO O PROCESSAMENTO {0}", DateTime.Now.ToString());

                Factoty.Factory fabrica = new Factoty.Factory();
                IDevice device = null;
                BaseMonitoramento monitoramento = null;

                if (ConfigurationManager.AppSettings["ReaderFromFile"].ToString() == "false")
                {
                    device = fabrica.GerarInstanciaDevice(TipoDevice.DeviceOnLine_ISO8583);
                }
                else
                {
                    device = fabrica.GerarInstanciaDevice(TipoDevice.DeviseOffLine_ISO8583);
                }

                PacketDevice selectedDevice = device.GerarPacketDevice();

                monitoramento = fabrica.GerarInstanciaMonitaramento(TipoMonitoramento.MonitorarTCP, selectedDevice);

                monitoramento.Monitorar();

                Console.WriteLine("FIM O PROCESSAMENTO {0}", DateTime.Now.ToString());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace); 
            }

           
           
        }
       

    }
}
