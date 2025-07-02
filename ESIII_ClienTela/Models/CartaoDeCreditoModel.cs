namespace ESIII_ClienTela.Models
{
    public class CartaoDeCreditoModel : EntidadeDominio
    {
        public int Cliente_id { get; set; }
        public required string Numero { get; set; }
        public required string NomeImpresso { get; set; }
        public required string CodSeguranca { get; set; }
        public required string Band { get; set; }
        public bool Preferencial { get; set; }
    }
}
