using NPRClient.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPRClient.Repositorio
{
    public interface IRepositorio
    {
        void Armazenar(List<IValueObject> pListaVO);

        void GerarAptadorArmazenamentoPorSingleton();

        void EncerrarAptadorArmazenamentoPorSingleton();
    }
}
