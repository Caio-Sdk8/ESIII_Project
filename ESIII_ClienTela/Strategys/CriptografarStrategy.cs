using ESIII_ClienTela.Models;

namespace ESIII_ClienTela.Strategys
{
    public class CriptografarStrategy : IStrategy<ClienteModel>
    {
        public string Processar(ClienteModel cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.Senha))
                return "Senha não pode estar vazia.";
            try
            {
                cliente.Senha = BCrypt.Net.BCrypt.HashPassword(cliente.Senha);
            }
            catch (Exception ex) {
                return ex.Message;
            }
            return "";
        }
    }
}
