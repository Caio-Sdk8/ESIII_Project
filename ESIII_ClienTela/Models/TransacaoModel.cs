namespace ESIII_ClienTela.Models
{
    public class TransacaoModel : EntidadeDominio
    {
        public required int Cliente_id { get; set; }
        public required DateTime Data {  get; set; }
        public required float Valor { get; set; }
    }
}
