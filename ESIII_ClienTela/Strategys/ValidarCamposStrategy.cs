using ESIII_ClienTela.Models;
using System.Reflection;

namespace ESIII_ClienTela.Strategys
{
    public class ValidarCamposStrategy : IStrategy<EntidadeDominio>
    {
        public string Processar(EntidadeDominio Entidade)
        {
            string erros = "";
            var propriedades = Entidade.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in propriedades)
            {
                var valor = prop.GetValue(Entidade);
                var nome = prop.Name;

                if (prop.PropertyType == typeof(string))
                {
                    if (string.IsNullOrWhiteSpace((string?)valor))
                        erros += $"Campo '{nome}' é obrigatório. ";
                }
                else if (prop.PropertyType.IsValueType)
                {
                    object defaultValue = Activator.CreateInstance(prop.PropertyType)!;
                    if (valor != null && valor.Equals(defaultValue))
                        erros += $"Campo '{nome}' está com valor padrão. ";
                }
                else
                {
                    if (valor == null)
                        erros += $"Campo '{nome}' não pode ser nulo. ";
                }
            }

            return string.IsNullOrEmpty(erros) ? "OK" : $"Erros: {erros.Trim()}";
        }
    }
}
