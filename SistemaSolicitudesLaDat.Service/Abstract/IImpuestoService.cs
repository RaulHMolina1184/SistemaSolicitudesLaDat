using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaSolicitudesLaDat.Entities.Desgloses;

namespace SistemaSolicitudesLaDat.Service.Abstract
{
    public interface IImpuestoService
    {
        Task<IEnumerable<ImpuestoValorAgregado>> ObtenerTodosIVAAsync();

    }
}
