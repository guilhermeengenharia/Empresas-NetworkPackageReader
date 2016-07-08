using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPRClient.ValueObject
{
    public class MensagemISO8583 : IValueObject
    {
        public string MensagemProcolo { get; private set; }
        public List<ItemMensagemISO8583> AtributosMensagem { get; set; }

        public override string ToString()
        {
            return MensagemProcolo;
        }

        public string ToTrasactSQL()
        {
            return string.Empty;
        }

        public void SetMensagemProtocolo(string pMensagem)
        {
            MensagemProcolo = pMensagem;
        }

        public void SetAtributoMensagem(ItemMensagemISO8583 pItemMensagem)
        {
            if (AtributosMensagem == null) AtributosMensagem = new List<ItemMensagemISO8583>();

            AtributosMensagem.Add(pItemMensagem);
        }

        public string GetSubValoresMensagemProtocolo(int pPosicaoInicial, int pPosicaoFinal)
        {
            return this.MensagemProcolo.Substring(pPosicaoFinal, pPosicaoFinal);
        }

    }
}
