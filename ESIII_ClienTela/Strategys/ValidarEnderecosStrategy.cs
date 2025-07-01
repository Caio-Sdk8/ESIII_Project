using ESIII_ClienTela.Models;

namespace ESIII_ClienTela.Strategys
{
    public class ValidarEnderecosStrategy : IStrategy<ClienteModel>
    {
        public List<EnderecoModel> Enderecos { get; set; }
        public string Processar(ClienteModel Entidade)
        {
            bool temEntrega = Enderecos.Any(e => e.TipoEndereco_id == 1);
            bool temCobranca = Enderecos.Any(e => e.TipoEndereco_id == 2);

            if (!temEntrega || !temCobranca)
                return $"Erro: O cliente {Entidade.Nome} precisa ter ao menos um endereço de entrega e um de cobrança.";

            return "OK";
        }
    }
}
