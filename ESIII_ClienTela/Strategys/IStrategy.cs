using ESIII_ClienTela.Models;

namespace ESIII_ClienTela.Strategys
{
    public interface IStrategy<T> where T : EntidadeDominio
    {
        string Processar(T Entidade);
    }
}
