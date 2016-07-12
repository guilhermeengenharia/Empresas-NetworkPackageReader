using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPRClient.ValueObject;
using NPRClient.ENUN;
using System.Xml;

namespace NPRClient.Conversores
{
    public class ConversorIso8583 : IConversor
    {

        private List<int> ListaTipoAtributos { get; set; }
        private MensagemISO8583 MensagemProtocolo { get; set; }

        public IValueObject ConverterMesangemParaVO(string pMensagem)
        {
            Factoty.Factory Fabrica = new Factoty.Factory();
            MensagemProtocolo = Fabrica.GerarInstanciaValueObject(TipoValueObject.Mensagem_ISO8583) as MensagemISO8583;

            MensagemProtocolo.SetMensagemProtocolo(pMensagem);

            DecodificarMensagem();

            MensagemProtocolo.SetRestrigirLimitacaoArmazenamento(900);

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

            return pTamanhoPosicional;
        }

        private int GerarBitMap(int pTamanhoPosicional)
        {
            string ValorBinario = string.Empty;
            string ValorBitMap = string.Empty;
            int ValorReferencia = 0;
            int ValorTamanhoBits = 0;

            ValorReferencia = Convert.ToInt32(MensagemProtocolo.GetSubValoresMensagemProtocolo(pTamanhoPosicional, 2),16);

            ValorBinario = Convert.ToString(ValorReferencia, 2);

            ValorTamanhoBits = (ValorBinario.First() == '1' ? 128 : 64);

            ValorBinario = string.Empty;

            for (int i = 0; i < ((ValorTamanhoBits / 4) -1); i += 2)
            {
                ValorReferencia = Convert.ToInt32(MensagemProtocolo.GetSubValoresMensagemProtocolo((pTamanhoPosicional + i), 2),16);

                ValorBinario += Convert.ToString(ValorReferencia, 2).PadLeft(8,'0');
                
            }

            ValorBitMap = MensagemProtocolo.GetSubValoresMensagemProtocolo(pTamanhoPosicional, (ValorTamanhoBits / 4));

            MensagemProtocolo.SetAtributoMensagem(new ItemMensagemISO8583() {TipoAtributo = TipoAtributoIso8583.BitMap_Binario, Valor = ValorBinario });

            MensagemProtocolo.SetAtributoMensagem(new ItemMensagemISO8583() { TipoAtributo = TipoAtributoIso8583.BitMap, Valor = ValorBitMap });

            pTamanhoPosicional = pTamanhoPosicional + (ValorTamanhoBits == 128 ? (ValorTamanhoBits / 8) : (ValorTamanhoBits / 4));

            SetListaTipoAtributos(ValorBinario, ValorTamanhoBits);

            return pTamanhoPosicional;
        }

        private void SetListaTipoAtributos(string pBinario,int pTamanhoBits)
        {
            ListaTipoAtributos = new List<int>();

            for (int i = 0; i <= (pTamanhoBits - 1); i++)
            {
                if (pBinario.Substring(i, 1) == "1")
                {
                    ListaTipoAtributos.Add((i+1));
                }
            }
        }

        private int GerarAtributos(int pTamanhoPosicional)
        {
            System.Xml.XmlDocument MapeamentoAtributos = new System.Xml.XmlDocument();

            MapeamentoAtributos.Load(@"Mapeamento/ISO8583.xml");


            for(int index = 0; index < ListaTipoAtributos.Count;index++)
            {
                XmlNode NodeAtributo = MapeamentoAtributos.SelectSingleNode("/ISO8583/ATRIBUTOS/ATRIBUTO[ID='" + ListaTipoAtributos[index].ToString() + "']");
                if (NodeAtributo != null)
                {
                    try
                    {

                        int tamanhoAtual = MensagemProtocolo.GetLeghtForPosicionStart(pTamanhoPosicional);
                        int tamanhoMensagem = int.Parse(NodeAtributo.LastChild.InnerText);

                        string valor = string.Empty;

                        if (tamanhoMensagem < 900)
                        {
                            if (tamanhoAtual >= tamanhoMensagem)
                            {
                                valor = MensagemProtocolo.GetSubValoresMensagemProtocolo(pTamanhoPosicional, tamanhoMensagem);
                            }
                            else
                            {
                                valor = string.Empty;
                            }
                            
                            pTamanhoPosicional += tamanhoMensagem;
                        }
                        else
                        {
                            if ( tamanhoAtual >= (pTamanhoPosicional + (tamanhoMensagem - 900)) )
                            {
                                string valorVariavel = MensagemProtocolo.GetSubValoresMensagemProtocolo(pTamanhoPosicional, (tamanhoMensagem - 900));
                                int tamanhoVariavel = 0;

                                valorVariavel = RetornarApenasNumero(valorVariavel);

                                pTamanhoPosicional += (tamanhoMensagem - 900);
                                tamanhoVariavel = int.Parse(valorVariavel);

                                if (MensagemProtocolo.GetLeghtForPosicionStart(pTamanhoPosicional) > tamanhoVariavel)
                                {
                                    valor = MensagemProtocolo.GetSubValoresMensagemProtocolo(pTamanhoPosicional, tamanhoVariavel);
                                }
                                else
                                {
                                    tamanhoVariavel = MensagemProtocolo.GetLeghtForPosicionStart(pTamanhoPosicional);
                                    try
                                    {
                                        valor = MensagemProtocolo.GetSubValoresMensagemProtocolo(pTamanhoPosicional, tamanhoVariavel);
                                    }
                                    catch
                                    {
                                        valor = string.Empty;
                                    }

                                }


                                pTamanhoPosicional += tamanhoVariavel;
                            }
                            else
                            {
                                valor = string.Empty;
                            }
                           

                        }


                        MensagemProtocolo.SetAtributoMensagem(new ItemMensagemISO8583() { TipoAtributo = (TipoAtributoIso8583)ListaTipoAtributos[index], Valor = valor });
                    }

                    catch (Exception ex)
                    {
                        throw new Exception(NodeAtributo.InnerText + " ex=" + ex.Message);
                    }
                
                }
            }


            return pTamanhoPosicional;

        }

        private string RetornarApenasNumero(string pValor)
        {
            string retorno = string.Join("", System.Text.RegularExpressions.Regex.Split(pValor, @"[^\d]"));

            if (retorno.Length == 0) return "0";

            return retorno;

            //var regex = new System.Text.RegularExpressions.Regex(@"\d+");
            //return regex.Replace(pValor, "");

        }
    }
}
