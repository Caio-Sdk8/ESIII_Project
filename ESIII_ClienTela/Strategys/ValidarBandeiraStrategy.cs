using ESIII_ClienTela.Enums;
using ESIII_ClienTela.Models;

namespace ESIII_ClienTela.Strategys
{
    public class ValidarBandeiraStrategy : IStrategy<CartaoDeCreditoModel>
    {
        public string Processar(CartaoDeCreditoModel Entidade)
        {
            bool bandeiraValida = Enum.TryParse(typeof(BandeiraCartaoEnum), Entidade.Band, ignoreCase: true, out _);

            return bandeiraValida ? "" : $"Erro: Bandeira '{Entidade.Band}' não é reconhecida.";
        }
    }
}
