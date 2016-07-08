using NPRClient.Repositorio;
using NPRClient.ValueObject;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NPRClient.Monitoramento
{
    public class MonitoramentoTCP_ISO8583 : BaseMonitoramento, IMonitoramento
    {
        protected PacketCommunicator Communicator { get; private set; }
        protected int LimiteSnapShot { get; private set; }

        protected int LimiteTimeout { get; private set; }

        public MonitoramentoTCP_ISO8583(PacketDevice pPackedDevice) :base(pPackedDevice)
        {
            LimiteSnapShot = 65536;
            LimiteTimeout = 0;
        }

        protected override void AbrirComunicacao()
        {
            base.AbrirComunicacao();

            // Abre a comunicação
            Communicator = base.SelectedDevice.Open(LimiteSnapShot, PacketDeviceOpenAttributes.MaximumResponsiveness, LimiteTimeout);           

            // Realizar Filtro de leitura
            //using (BerkeleyPacketFilter filter = communicator.CreateFilter("ip and udp"))
            //using (BerkeleyPacketFilter filter = communicator.CreateFilter("tcp port 23"))

            if (ConfigurationManager.AppSettings["ReaderFromFile"].ToString() == "false")
            {
                using (BerkeleyPacketFilter filter = Communicator.CreateFilter(ConfigurationManager.AppSettings["FilterPacket"].ToString()))
                {
                    // Inclui filtro configurado
                    Communicator.SetFilter(filter);
                }
            }

            Console.WriteLine("Realizando leitura de pacotes " + SelectedDevice.Description + "...");

            try
            {
                // Iniciar a captura de pacotes
                Communicator.ReceivePackets(0, CapturarPacote);
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
                       
                
        }

        protected override void CapturarPacote(Packet packet)
        {
            
            if ((base.PossuiIntervaloMonitoramento == false) || (base.FimMonitoramento >= DateTime.Now))
            {
                List<IValueObject> Coletagem = new List<IValueObject>();
               
                try
                {

                    Console.WriteLine(packet.Timestamp.ToString("yyyy-MM-dd hh:mm:ss.fff") + " length:" + packet.Length);

                    IpV4Datagram ip = packet.Ethernet.IpV4;
                    PcapDotNet.Packets.Transport.TcpDatagram tcp = ip.Tcp;

                    Console.WriteLine("IP Source: " + ip.Source + ":" + tcp.SourcePort + " -> " + "IP Destination: " + ip.Destination + ":" + tcp.DestinationPort);

                    if ((tcp.Payload != null) && (Encoding.ASCII.GetString(tcp.Payload.ToArray()).Trim() != ""))
                    {

                        ProtocoloTCP_ISO8583 item = Fabrica.GerarInstanciaValueObject(ENUN.TipoValueObject.ProtocoloTCP_ISO8583) as ProtocoloTCP_ISO8583;
                        string mensagem = RemoveInvalidCharacters(Encoding.ASCII.GetString(tcp.Payload.ToArray()).Trim()).Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("|", "").Replace("?", "").Replace(";", "");

                        //if (item.TryParse(tcp.Payload.ToArray()))
                        //{
                            item.Time = packet.Timestamp;
                            item.IP_Origem = ip.Source.ToString();
                            item.TCP_Destino = tcp.SourcePort.ToString();
                            item.IP_Destino = ip.Destination.ToString();
                            item.TCP_Destino = tcp.DestinationPort.ToString();
                            item.MensagemProcolo = Conversor.ConverterMesangemParaVO(mensagem) as MensagemISO8583;

                            Coletagem.Add(item);
                        //}


                    }                  

                    Repositorio.Armazenar(Coletagem);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    Coletagem = null;
                                   }
            }
            else
            {
                Console.WriteLine("Suspender Monitoramento");               
                Communicator.Break();
            }
       
           
            
        }

    }
}
