using ESIII_ClienTela.DAO;
using ESIII_ClienTela.Models;
using System.Security.Cryptography;

namespace ESIII_ClienTela.Strategys
{
    public class GerarRankingStrategy : IStrategy<ClienteModel>
    {
        readonly ClienteDAO CliDao = new();
        public string Processar(ClienteModel Entidade)
        {
            for (int i = 0; i < 100; i++)
            {
                int ranking = RandomNumberGenerator.GetInt32(1, 100000);

                if (!CliDao.ExisteRanking(ranking))
                {
                    return ranking.ToString();
                }
            }

            return "Erro: Não foi possível gerar um ranking único.";
        }
    }
}
