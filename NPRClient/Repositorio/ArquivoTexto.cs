using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPRClient.Repositorio
{
    public class ArquivoTexto : IRepositorio
    {
        
        public async void Armazenar(List<ValueObject.IValueObject> pListaVO)
        {
            if(pListaVO != null && pListaVO.Count > 0)
            {
                string strRepositoryFile = ConfigurationManager.AppSettings["RepositoryFiles"].ToString() + DateTime.Now.ToString("yyyyMMdd") + ".txt";

                string ConteudoArquivo = "";

                foreach (ValueObject.IValueObject item in pListaVO)
                {
                    ConteudoArquivo = JsonConvert.SerializeObject(item);
                    ConteudoArquivo += Environment.NewLine;
                }

                File.AppendAllText(strRepositoryFile, ConteudoArquivo);
            }
            
        }

        public void EncerrarAptadorArmazenamentoPorSingleton()
        {
            
        }

        public void GerarAptadorArmazenamentoPorSingleton()
        {
            
        }

    }
}
