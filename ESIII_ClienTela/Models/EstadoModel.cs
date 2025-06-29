namespace ESIII_ClienTela.Models
{
    public class EstadoModel : EntidadeDominio
    {
        public required string Nome { get; set; }
        public required string Uf { get; set; }
        public required int Pais_id { get; set; }

    }
}
