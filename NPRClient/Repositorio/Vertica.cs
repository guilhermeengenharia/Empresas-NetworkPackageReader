using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertica.Data;

namespace NPRClient.Repositorio
{
    public class VerticaRepositorio : IRepositorio
    {

        public VerticaRepositorio()
        {
           
        }

        public async void Armazenar(List<ValueObject.IValueObject> pListaVO)
        {
            try
            {

                if (pListaVO != null && pListaVO.Count > 0)
                {
                   
                    foreach (ValueObject.IValueObject item in pListaVO)
                    {
                        ExecutarComando(item.ToTrasactSQL());
                    }

                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }

        protected void ExecutarComando(string pComando)
        {
            var Comando = BancoDadosVertica.AbrirConexao().CreateCommand();

            Comando.CommandText = pComando;

            Comando.ExecuteNonQuery();
        }

        public void GerarAptadorArmazenamentoPorSingleton()
        {
            BancoDadosVertica.AbrirConexao();
        }

        public void EncerrarAptadorArmazenamentoPorSingleton()
        {
            BancoDadosVertica.FecharConexao();
        }
    }

    public class BancoDadosVertica
    {
        private static Vertica.Data.VerticaClient.VerticaConnection _instance;

        public static Vertica.Data.VerticaClient.VerticaConnection AbrirConexao()
        {
          
            var stringConexao = ConfigurationManager.ConnectionStrings["VerticaConnection"].ConnectionString;

            if (_instance == null)
            {
                _instance = new Vertica.Data.VerticaClient.VerticaConnection(stringConexao);
            }

            if (_instance.State == System.Data.ConnectionState.Closed)
            {
                _instance.OpenAsync();
            }
            
            return _instance;
        }

        public static void FecharConexao()
        {
            if (_instance != null)
            {
                _instance.Close();
            }

            _instance = null;
            
        }
    }
}
