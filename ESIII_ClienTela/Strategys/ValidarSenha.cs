using ESIII_ClienTela.Models;
using System.Text.RegularExpressions;

namespace ESIII_ClienTela.Strategys
{
    public class ValidarSenha : IStrategy<ClienteModel>
    {
        private static readonly Regex _regex = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).{8,}$");

        public string Processar(ClienteModel cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.Senha))
            {
                return "A senha não pode estar vazia.";
            }

            if (!_regex.IsMatch(cliente.Senha))
            {
                return "A senha deve conter ao menos 8 caracteres, uma letra maiúscula, uma minúscula e um caractere especial.";
            }

            return string.Empty;
        }
    }
}
