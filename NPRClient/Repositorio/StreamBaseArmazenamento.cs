using System;
using System.Collections.Generic;
using NPRClient.ValueObject;
using System.Configuration;
using StreamBase.SB.Client;
using System.Threading.Tasks;

namespace NPRClient.Repositorio
{
    public class StreamBaseArmazenamento : IRepositorio
    {

        public async void Armazenar(List<IValueObject> pListaVO)
        {
            if (pListaVO != null && pListaVO.Count > 0)
            {
                string ConteudoStreamBase = "";

                foreach (ValueObject.IValueObject item in pListaVO)
                {
                    ConteudoStreamBase += item.ToString();
                    ConteudoStreamBase += Environment.NewLine;

                    await Task.Run(() =>  EnviarParaFilaStreamBase(item));
                }



                //EnviarParaFilaStreamBase(ConteudoStreamBase);


            }
        }

        protected void EnviarParaFilaStreamBaseTeste(string pMensagemSnifer)
        {
            string ServerStreamBase = ConfigurationManager.AppSettings["StreamBaseServer"].ToString(); 
            string InputStreamBase = ConfigurationManager.AppSettings["StreamBaseInput"].ToString();
            string FieldNameStreamBase = ConfigurationManager.AppSettings["StreamBaseField"].ToString();

            StreamBaseClient ClientStreamBase = null;
            StreamBaseURI UriStreamBase = new StreamBaseURI(ServerStreamBase);
            ClientStreamBase = new StreamBaseClient(UriStreamBase);

            StreamBase.SB.Schema SchemaStreamBase = ClientStreamBase.GetSchemaForStream(InputStreamBase);

            StreamBase.SB.Tuple TuplaStreamBase = SchemaStreamBase.CreateTuple();
            StreamBase.SB.Schema.Field FieldStreamBase = SchemaStreamBase.GetField(FieldNameStreamBase);

            try
            {
                if (pMensagemSnifer.Length < 900)
                {
                    TuplaStreamBase.SetString(FieldStreamBase, pMensagemSnifer);

                }
                else
                {
                    TuplaStreamBase.SetString(FieldStreamBase, pMensagemSnifer.Substring(0, 900));
                }


                ClientStreamBase.Enqueue(InputStreamBase, TuplaStreamBase);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ServerStreamBase = null;
                InputStreamBase = null;
                FieldNameStreamBase = null;
                FieldStreamBase = null;
                TuplaStreamBase = null;
                SchemaStreamBase = null;
                UriStreamBase = null;
                ClientStreamBase = null;
            }


        }

        protected  void EnviarParaFilaStreamBase(IValueObject item)
        {
            string InputStreamBase = "Input_Read_Stream_Base";//ConfigurationManager.AppSettings["StreamBaseInput"].ToString();
                                                              //string FieldNameStreamBase = "";//ConfigurationManager.AppSettings["StreamBaseField"].ToString();


            StreamBase.SB.Schema SchemaStreamBase = ApdadorStreamBase.AbrirComunicacao().GetSchemaForStream(InputStreamBase);

            StreamBase.SB.Tuple TuplaStreamBase = SchemaStreamBase.CreateTuple();

            StreamBase.SB.Schema.Field Field_IP_ORIGEM = SchemaStreamBase.GetField("IP_ORIGEM");
            StreamBase.SB.Schema.Field Field_PORTA_ORIGEM = SchemaStreamBase.GetField("PORTA_ORIGEM");
            StreamBase.SB.Schema.Field Field_IP_DESTINO = SchemaStreamBase.GetField("IP_DESTINO");
            StreamBase.SB.Schema.Field Field_PORTA_DESTINO = SchemaStreamBase.GetField("PORTA_DESTINO");
            StreamBase.SB.Schema.Field Field_DT_MONITORAMENTO = SchemaStreamBase.GetField("DT_MONITORAMENTO");
            StreamBase.SB.Schema.Field Field_DS_MENSAGEM_PROTOCOLO = SchemaStreamBase.GetField("DS_MENSAGEM_PROTOCOLO");

            StreamBase.SB.Timestamp DT_MONITORAMENTO = StreamBase.SB.Timestamp.Now();

            var itemMonitoramento = item as ValueObject.ProtocoloTCP_ISO8583;
            try
            {
                TuplaStreamBase.SetString(Field_IP_ORIGEM, itemMonitoramento.IP_Origem);
                TuplaStreamBase.SetString(Field_PORTA_ORIGEM, itemMonitoramento.TCP_Origem);

                TuplaStreamBase.SetString(Field_IP_DESTINO, itemMonitoramento.IP_Destino);
                TuplaStreamBase.SetString(Field_PORTA_DESTINO, itemMonitoramento.TCP_Destino);

                TuplaStreamBase.SetTimestamp(Field_DT_MONITORAMENTO, DT_MONITORAMENTO);


                if (itemMonitoramento.MensagemProcolo.Length < 900)
                {
                    TuplaStreamBase.SetString(Field_DS_MENSAGEM_PROTOCOLO, itemMonitoramento.MensagemProcolo);
                }
                else
                {
                    TuplaStreamBase.SetString(Field_DS_MENSAGEM_PROTOCOLO, itemMonitoramento.MensagemProcolo.Substring(0, 900));
                }


                ApdadorStreamBase.Enfilerar(InputStreamBase, TuplaStreamBase);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
                TuplaStreamBase = null;
                SchemaStreamBase = null;
               
                Field_IP_ORIGEM = null;
                Field_PORTA_ORIGEM = null;
                Field_IP_DESTINO = null;
                Field_PORTA_DESTINO = null;
                Field_DT_MONITORAMENTO = null;
                Field_DS_MENSAGEM_PROTOCOLO = null;

            }


        }
             

        public void GerarAptadorArmazenamentoPorSingleton()
        {
            ApdadorStreamBase.AbrirComunicacao();
        }

        public void EncerrarAptadorArmazenamentoPorSingleton()
        {
            ApdadorStreamBase.DestruirSingleton();
        }
    }


    public class ApdadorStreamBase
    {
        private static StreamBaseClient _instance { get; set; }

        public static StreamBaseClient AbrirComunicacao()
        {
            if (_instance == null)
            {
                string ServerStreamBase = ObterServerStreamBase();
                StreamBaseURI UriStreamBase = new StreamBaseURI(ServerStreamBase);
                _instance = new StreamBaseClient(UriStreamBase);
            }

            return _instance;

        }

        public static void Enfilerar(string pInputStreamBase, StreamBase.SB.Tuple pTupla)
        {            
            AbrirComunicacao().Enqueue(pInputStreamBase,pTupla);
        }

        public static void DestruirSingleton()
        {
            _instance = null;
        }
        private static string ObterServerStreamBase()
        {
            string ServerStreamBase = ConfigurationManager.AppSettings["StreamBaseServer"].ToString();
            string ServerLocal = System.Environment.GetEnvironmentVariable("STREAMBASE_SERVER");

            if (ServerLocal != null)
            {
                ServerStreamBase = ServerLocal;
            }

            return ServerStreamBase;
        }

    }


}
