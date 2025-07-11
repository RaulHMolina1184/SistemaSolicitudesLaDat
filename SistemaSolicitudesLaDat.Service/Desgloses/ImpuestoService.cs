using SistemaSolicitudesLaDat.Entities.Desgloses;
using SistemaSolicitudesLaDat.Repository.Desgloses;
using SistemaSolicitudesLaDat.Repository.Infrastructure;
using SistemaSolicitudesLaDat.Service.Abstract;
using SistemaSolicitudesLaDat.Service.Bitacora;

namespace SistemaSolicitudesLaDat.Service.Desgloses
{
    public class ImpuestoService : IImpuestoService
    {
        private readonly ImpuestoRepository _impuestoRepository;
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public ImpuestoService(IDbConnectionFactory dbConnectionFactory, ImpuestoRepository impuestoRepository)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _impuestoRepository = impuestoRepository;
        }
        public async Task<IEnumerable<ImpuestoValorAgregado>> ObtenerTodosIVAAsync()
        {
            var ivas = await _impuestoRepository.ObtenerTodosAsync();
            return ivas;
        }
    }
}
