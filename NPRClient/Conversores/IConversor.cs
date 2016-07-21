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
        bool GetConversaoEfetuadaSucesso();
        IValueObject ConverterMesangemParaVO(string pMensagem);

        bool ValidarFormatoConversao(string atributo, int tipoAtributo);
    }
}
