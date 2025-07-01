using ESIII_ClienTela.DAO;
using ESIII_ClienTela.Models;
using Google.Protobuf.WellKnownTypes;

namespace ESIII_ClienTela.Strategys
{
    public class ValidarExistenciaEmailCpfStrategy : IStrategy<ClienteModel>
    {
        readonly ClienteDAO CliDao = new();
        public string Processar(ClienteModel Entidade)
        {
            List<ClienteModel> BuscaEmail = CliDao.BuscarClientesParams(null, Entidade.Email, null, null);
            List<ClienteModel> BuscaCpf = CliDao.BuscarClientesParams(null, null, Entidade.Cpf, null);

            if (BuscaCpf.Count > 0 || BuscaEmail?.Count > 0)
            {
                return "E-mail ou Cpf já cadastrados";
            }

            return "";
        }
    }
}
