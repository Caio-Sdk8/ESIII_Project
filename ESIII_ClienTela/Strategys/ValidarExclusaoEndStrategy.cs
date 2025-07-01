using ESIII_ClienTela.DAO;
using ESIII_ClienTela.Models;

namespace ESIII_ClienTela.Strategys
{
    public class ValidarExclusaoEndStrategy : IStrategy<EnderecoModel>
    {
        readonly EnderecoDAO EndDao = new();
        public string Processar(EnderecoModel Entidade)
        {
            List<EnderecoModel> EnderecosCli = EndDao.BuscarPorClienteId(Entidade.Cliente_id);

            bool liberarExclusao = EnderecosCli.Any(e => e.TipoEndereco_id == Entidade.TipoEndereco_id);

            if (!liberarExclusao)
            {
                return "Erro: este endereço não pode ser excluido pois deve exister ao menos um de cada um dos tipos (cobrança e entrega)";

            }

            return "";
        }
    }
}
