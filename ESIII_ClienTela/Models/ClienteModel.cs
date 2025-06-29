
namespace ESIII_ClienTela.Models
{
    public class ClienteModel : EntidadeDominio
    {
        public required string Nome { get; set; }
        public required string Genero { get; set; }
        public DateOnly DataNascimento { get; set; }
        public required string Cpf { get; set; }
        public required string Email { get; set; }
        public required string Senha { get; set; }
        public required string Status { get; set; }
        public int Ranking { get; set; }
    }
}
