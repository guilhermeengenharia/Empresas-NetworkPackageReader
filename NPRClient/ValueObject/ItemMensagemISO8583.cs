using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPRClient.ValueObject
{

    public class ItemMensagemISO8583 : IValueObject
    {

        public ENUN.TipoAtributoIso8583 TipoAtributo { get; set; }

        public string Valor { get; set; }

        public string ToTrasactSQL()
        {
            return string.Empty;
        }

    }
}
