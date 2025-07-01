namespace ESIII_ClienTela.Models
{
    public class EnderecoModel : EntidadeDominio
    {
        public required int Cliente_id { get; set; }
        public required int Cidade_id { get; set; }
        public required int TipoLogradouro_id { get; set; }
        public required int TipoResidencia_id { get; set; }
        public required int TipoEndereco_id { get; set; }
        public required string Apelido { get; set; }
        public required string Logradouro { get; set; }
        public required string Numero { get; set; }
        public required string Bairro { get; set; }
        public required string Cep { get; set; }
        public required string Obs { get; set; }
    }
}
