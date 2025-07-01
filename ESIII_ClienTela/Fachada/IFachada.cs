using ESIII_ClienTela.Models;

namespace ESIII_ClienTela.Fachada
{
    public interface IFachada<T> where T : EntidadeDominio
    {
        string salvar(T entidade);
        T alterar(T entidade);
        string excluir(T entidade);
        List<T> consultar(T entidade);

    }
}
