using ESIII_ClienTela.Models;

namespace ESIII_ClienTela.Strategys
{
    public class ValidarEnderecosStrategy : IStrategy<ClienteModel>
    {
        public string Processar(ClienteModel Entidade)
        {
            bool temEntrega = Entidade.Enderecos.Any(e => e.TipoEndereco_id == 1);
            bool temCobranca = Entidade.Enderecos.Any(e => e.TipoEndereco_id == 2);

            if (!temEntrega || !temCobranca)
                return $"Erro: O cliente {Entidade.Nome} precisa ter ao menos um endereço de entrega e um de cobrança.";

            return "";
        }
    }
}
