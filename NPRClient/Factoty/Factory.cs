﻿using NPRClient.Conversores;
using NPRClient.ENUN;
using NPRClient.Monitoramento;
using NPRClient.Repositorio;
using NPRClient.ValueObject;

namespace NPRClient.Factoty
{
    public class Factory
    {

        public BaseMonitoramento GerarInstanciaMonitaramento(TipoMonitoramento pTipoMonitoramento, PcapDotNet.Core.PacketDevice pPackedDevice)
        {
            BaseMonitoramento instancia = null;

            switch (pTipoMonitoramento)
            {
                case (TipoMonitoramento.MonitorarTCP) :
                    instancia = new MonitoramentoTCP_ISO8583(pPackedDevice);
                    break;
                default:
                    instancia = null;
                    break;
                
            }

            return instancia;
        }

        public IRepositorio GerarInstanciaRepositorio(TipoRepositorio pTipoRepositorio)
        {
            IRepositorio instancia = null;

            switch (pTipoRepositorio)
            {
                case(TipoRepositorio.ArquivoTexto) :
                    instancia = new ArquivoTexto();
                    break;
                case(TipoRepositorio.Vertica):
                    instancia = new VerticaRepositorio();
                    break;
                case (TipoRepositorio.StreamBase):
                    instancia = new StreamBaseArmazenamento();
                    break;
                default:
                    instancia = null;
                    break;
            }
            return instancia;
        }

        public IValueObject GerarInstanciaValueObject(TipoValueObject pTipoValueObject)
        {
            IValueObject instancia = null;

            switch (pTipoValueObject)
            {
                case (TipoValueObject.ProtocoloTCP_ISO8583):
                    instancia = new ProtocoloTCP_ISO8583();
                    break;
                case (TipoValueObject.Mensagem_ISO8583):
                    instancia = new MensagemISO8583();
                    break;
                case (TipoValueObject.ItemMensagem_ISO8583):
                    instancia = new ItemMensagemISO8583();
                    break;
                default:
                    instancia = null;
                    break;
            }

            return instancia;
        }

        public IDevice GerarInstanciaDevice(TipoDevice pTipoDevice)
        {
            IDevice instancia = null;

            switch (pTipoDevice)
            {
                case(TipoDevice.DeviseOffLine_ISO8583) :
                    instancia = new DeviceOffLine_ISO8583();
                    break;
                case(TipoDevice.DeviceOnLine_ISO8583) :
                    instancia = new DeviceOnLine_ISO8583();
                    break;
                default:
                    instancia = null;
                    break;

            }
            return instancia;
        }


        public NPRClient.Conversores.IConversor GerarInstanciaConversor (TipoConversor pTipoConversor)
        {
            NPRClient.Conversores.IConversor instancia = null;

            switch (pTipoConversor)
            {
                case (TipoConversor.Conversor_ISO8583):
                    instancia = new NPRClient.Conversores.ConversorIso8583();
                    break;                
                default:
                    instancia = null;
                    break;
            }
            return instancia;
        }
    }
}
