namespace ESIII_ClienTela.Models
{
    public class ClienteTeste : EntidadeDominio
    {
        public string Nome { get; set; }

        public required string Genero { get; set; }

        public DateTime DataNascimento { get; set; }

        public required string Cpf { get; set; }

        public required string Email { get; set; }
        public required string Senha { get; set; }
        public required Boolean Status { get; set; }
        public int Ranking { get; set; }

        //public List<TelefoneModel> Telefones { get; set; } = new();
        //public List<EnderecoModel> Enderecos { get; set; } = new();
        //public List<CartaoDeCreditoModel> Cartoes { get; set; } = new();
    }
}