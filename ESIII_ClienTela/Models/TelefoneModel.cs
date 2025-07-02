namespace ESIII_ClienTela.Models
{
    public class TelefoneModel : EntidadeDominio
    {
        public int Cliente_id { get; set; }
        public required int TipoTelefone_id { get; set; }
        public required string Ddd { get; set; }
        public required string Numero { get; set; }
    }
}
