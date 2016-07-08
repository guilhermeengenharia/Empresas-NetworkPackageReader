using NPRClient.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPRClient.Conversores
{
    public interface IConversor
    {

        IValueObject ConverterMesangemParaVO(string pMensagem);
    }
}
