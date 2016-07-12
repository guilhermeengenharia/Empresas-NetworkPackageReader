using System;
using System.Collections.Generic;
using NPRClient.ValueObject;
using System.Configuration;
using StreamBase.SB.Client;
using System.Threading.Tasks;

namespace NPRClient.Repositorio
{
    public class StreamBaseForJSon : IRepositorio
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

                    await Task.Run(() => EnviarParaFilaStreamBase(item));
                }



                //EnviarParaFilaStreamBase(ConteudoStreamBase);


            }
        }

        
        protected void EnviarParaFilaStreamBase(IValueObject item)
        {
            string InputStreamBase = "Input_Sniffer_JSON";//ConfigurationManager.AppSettings["StreamBaseInput"].ToString();
                                                              //string FieldNameStreamBase = "";//ConfigurationManager.AppSettings["StreamBaseField"].ToString();


            StreamBase.SB.Schema SchemaStreamBase = ApdadorStreamBase.AbrirComunicacao().GetSchemaForStream(InputStreamBase);

            StreamBase.SB.Tuple TuplaStreamBase = SchemaStreamBase.CreateTuple();

            StreamBase.SB.Schema.Field Field_Sniffer_JSON = SchemaStreamBase.GetField("Sniffer_JSON");
            
            try
            {
                TuplaStreamBase.SetString(Field_Sniffer_JSON, Newtonsoft.Json.JsonConvert.SerializeObject(item));
               
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
                Field_Sniffer_JSON = null;                
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
}
