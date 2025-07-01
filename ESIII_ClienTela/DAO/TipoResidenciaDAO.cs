using MySql.Data.MySqlClient;
using ESIII_ClienTela.Data;
using ESIII_ClienTela.Models;

namespace ESIII_ClienTela.DAO
{
    public class TipoResidenciaDAO : IDAO<TipoResidenciaModel>
    {
        public TipoResidenciaModel ObterPorId(int id)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "SELECT * FROM TipoResidencia WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new TipoResidenciaModel
                {
                    Id = reader.GetInt32("id"),
                    Tipo = reader.GetString("tipo")
                };
            }
            return null;
        }

        public List<TipoResidenciaModel> ListarTodos()
        {
            var lista = new List<TipoResidenciaModel>();

            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "SELECT * FROM TipoResidencia";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new TipoResidenciaModel
                {
                    Id = reader.GetInt32("id"),
                    Tipo = reader.GetString("tipo")
                });
            }

            return lista;
        }

        public int Inserir(TipoResidenciaModel obj)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "INSERT INTO TipoResidencia (tipo) VALUES (@tipo); SELECT LAST_INSERT_ID()";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@tipo", obj.Tipo);

            int idGerado = Convert.ToInt32(cmd.ExecuteScalar());
            return idGerado;
        }

        public void Atualizar(TipoResidenciaModel obj)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "UPDATE TipoResidencia SET tipo = @tipo WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@tipo", obj.Tipo);
            cmd.Parameters.AddWithValue("@id", obj.Id);

            cmd.ExecuteNonQuery();
        }

        public void Remover(int id)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "DELETE FROM TipoResidencia WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}

