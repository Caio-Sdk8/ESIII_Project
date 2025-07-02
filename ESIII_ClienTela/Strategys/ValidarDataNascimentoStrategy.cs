using ESIII_ClienTela.Models;

namespace ESIII_ClienTela.Strategys
{
    public class ValidarDataNascimentoStrategy : IStrategy<ClienteModel>
    {
        public string Processar(ClienteModel Entidade)
        {
            if(Entidade.DataNascimento >= DateTime.Today)
            {
                return "A data de nascimento não pode ser igual ou posterior à data de hoje";
            }

            return "";
        }
    }
}
