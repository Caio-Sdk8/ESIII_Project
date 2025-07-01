namespace ESIII_ClienTela.Models
{
    public class ClienteCompletoViewModel
    {
        public required ClienteModel Cliente { get; set; }
        public required List<EnderecoModel> Enderecos { get; set; }
        public required List<CartaoDeCreditoModel> Cartoes { get; set; }
        public required List<TelefoneModel> Telefone { get; set; }
    }
}
