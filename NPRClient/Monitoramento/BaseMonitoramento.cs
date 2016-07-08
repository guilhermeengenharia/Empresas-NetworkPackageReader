using NPRClient.Conversores;
using NPRClient.Repositorio;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NPRClient.Monitoramento
{
    public abstract class BaseMonitoramento : IMonitoramento
    {
        public PacketDevice SelectedDevice { get; private set; }
        public DateTime InicioMonitoramento { get; private set; }
        public int MinutosParaMonitoramento { get; private set; }
        public DateTime FimMonitoramento { get; private set; }
        
        public bool PossuiIntervaloMonitoramento { get;private set; }

        public Factoty.Factory Fabrica { get;  set; }
        public IRepositorio Repositorio { get;  set; }
        public IConversor Conversor { get; set; }

        public NPRClient.ENUN.TipoRepositorio tRepositorio { get; private set; }

        public BaseMonitoramento(PacketDevice pPackedDevice)
        {
            SelectedDevice = pPackedDevice;
            InicioMonitoramento = DateTime.Now;
            MinutosParaMonitoramento = int.Parse(ConfigurationManager.AppSettings["IntervaloMonitoramento"].ToString());
            PossuiIntervaloMonitoramento = bool.Parse(ConfigurationManager.AppSettings["PossuiIntervaloMonitoramento"].ToString());

        }

        public virtual void Monitorar()
        {
            try
            {
                AbrirComunicacao();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DestruirInstancias();
            }
            
        }

        protected virtual void AbrirComunicacao()
        {
            CalcularFimProcessamento();
            GerarFabricaInstancia();
            GerarRepositorio();
            GerarConversor();
        }
        
        protected virtual void CapturarPacote(Packet packet)
        {
            throw new NotImplementedException();
        }

        protected string RemoveInvalidCharacters(string str)
        {
            var invalidXmlCharactersRegex = new Regex("[^\u0009\u000a\u000d\u0020-\ud7ff\ue000-\ufffd]|([\ud800-\udbff](?![\udc00-\udfff]))|((?<![\ud800-\udbff])[\udc00-\udfff])");
            return invalidXmlCharactersRegex.Replace(str, "");
        }

        protected void CalcularFimProcessamento()
        {
            FimMonitoramento = InicioMonitoramento.AddSeconds(MinutosParaMonitoramento);
        }

        private void GerarFabricaInstancia()
        {
            Fabrica = new Factoty.Factory();
        }

        private void GerarRepositorio()
        {
            tRepositorio = (NPRClient.ENUN.TipoRepositorio)int.Parse(ConfigurationManager.AppSettings["TipoRepositorio"].ToString());

            Repositorio = Fabrica.GerarInstanciaRepositorio(tRepositorio);

            Repositorio.GerarAptadorArmazenamentoPorSingleton();
        }

        private void GerarConversor()
        {
            Conversor = Fabrica.GerarInstanciaConversor(ENUN.TipoConversor.Conversor_ISO8583);
        }

        private  void DestruirInstancias()
        {
            Repositorio.EncerrarAptadorArmazenamentoPorSingleton();
            Repositorio = null;
            Fabrica = null;
        }
    }
}
