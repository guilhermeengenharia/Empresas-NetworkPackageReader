using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPRClient.ENUN
{

    public enum TipoMonitoramento
    {
        MonitorarTCP
    }

    public enum TipoRepositorio
    {
        ArquivoTexto,
        Vertica,
        StreamBase
    }

    public enum TipoValueObject
    {
        ProtocoloTCP_ISO8583
    }

    public enum TipoDevice
    {
        DeviseOffLine_ISO8583,
        DeviceOnLine_ISO8583
    }

   
}
