using ESIII_ClienTela.Models;
using ESIII_ClienTela.Models.DTOs;

namespace ESIII_ClienTela.Fachada
{
    public interface IFachada<T> where T : EntidadeDominio
    {
        ResponseDTO salvar(T entidade);
        ResponseDTO alterar(T entidade);
        string excluir(T entidade);
        List<T> consultar(T entidade);

    }
}
