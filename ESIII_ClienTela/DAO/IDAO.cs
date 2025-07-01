using ESIII_ClienTela.Models;

namespace ESIII_ClienTela.DAO
{
    public interface IDAO<Entidade> where Entidade : EntidadeDominio
    {
        Entidade ObterPorId(int id);
        List<Entidade> ListarTodos();
        int Inserir(Entidade entidade);
        void Atualizar(Entidade entidade);
        void Remover(int id);
    }
}
