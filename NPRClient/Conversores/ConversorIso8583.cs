using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPRClient.ValueObject;
using NPRClient.ENUN;

namespace NPRClient.Conversores
{
    public class ConversorIso8583 : IConversor
    {

        private List<TipoAtributoIso8583> ListaTipoAtributos { get; set; }
        private MensagemISO8583 MensagemProtocolo { get; set; }

        public IValueObject ConverterMesangemParaVO(string pMensagem)
        {
            Factoty.Factory Fabrica = new Factoty.Factory();
            MensagemProtocolo = Fabrica.GerarInstanciaValueObject(TipoValueObject.Mensagem_ISO8583) as MensagemISO8583;

            MensagemProtocolo.SetMensagemProtocolo(pMensagem);

            DecodificarMensagem();

            return MensagemProtocolo;
        }

        private void DecodificarMensagem()
        {
            int posicao = 4;

            posicao = GerarTipoMensagem(posicao);
            posicao = GerarBitMap(posicao);
            posicao = GerarAtributos(posicao);

        }

        private int GerarTipoMensagem(int pTamanhoPosicional)
        {

            string valor = MensagemProtocolo.GetSubValoresMensagemProtocolo(0, pTamanhoPosicional);

            MensagemProtocolo.SetAtributoMensagem(new ItemMensagemISO8583() { TipoAtributo = TipoAtributoIso8583.MensageType, Valor = valor });

            return pTamanhoPosicional + 1;
        }

        private int GerarBitMap(int pTamanhoPosicional)
        {
            string ValorBinario = string.Empty;
            string ValorBitMap = string.Empty;
            int ValorReferencia = 0;
            int ValorTamanhoBits = 0;

            ValorReferencia = int.Parse(MensagemProtocolo.GetSubValoresMensagemProtocolo(pTamanhoPosicional, 2));

            ValorBinario = Convert.ToString(ValorReferencia, 2);

            ValorTamanhoBits = (ValorBinario.First() == '1' ? 128 : 64);

            ValorBinario = string.Empty;

            for (int i = 0; i < (ValorTamanhoBits / 4); i += 2)
            {
                ValorReferencia = int.Parse(MensagemProtocolo.GetSubValoresMensagemProtocolo((pTamanhoPosicional + i), 2));

                ValorBinario += Convert.ToString(ValorReferencia, 2);
                
            }

            ValorBitMap = MensagemProtocolo.GetSubValoresMensagemProtocolo(pTamanhoPosicional, (ValorTamanhoBits / 4));

            MensagemProtocolo.SetAtributoMensagem(new ItemMensagemISO8583() {TipoAtributo = TipoAtributoIso8583.BitMap_Binario, Valor = ValorBinario });

            MensagemProtocolo.SetAtributoMensagem(new ItemMensagemISO8583() { TipoAtributo = TipoAtributoIso8583.BitMap2, Valor = ValorBitMap });

            pTamanhoPosicional = pTamanhoPosicional + (ValorTamanhoBits == 128 ? (ValorTamanhoBits / 8) : (ValorTamanhoBits / 4));

            SetListaTipoAtributos(ValorBinario, ValorTamanhoBits);

            return pTamanhoPosicional;
        }

        private void SetListaTipoAtributos(string pBinario,int pTamanhoBits)
        {
            for (int i = 0; i <= pTamanhoBits; i++)
            {
                if (pBinario.Substring(i, 1) == "1")
                {
                    ListaTipoAtributos.Add((TipoAtributoIso8583)i);
                }
            }
        }

        private int GerarAtributos(int pTamanhoPosicional)
        {

        }
    }
}
