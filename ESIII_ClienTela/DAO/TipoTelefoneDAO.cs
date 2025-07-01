using MySql.Data.MySqlClient;
using ESIII_ClienTela.Data;
using ESIII_ClienTela.Models;

namespace ESIII_ClienTela.DAO
{
    public class TipoTelefoneDAO : IDAO<TipoTelefoneModel>
    {
        public TipoTelefoneModel ObterPorId(int id)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "SELECT * FROM TipoTelefone WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new TipoTelefoneModel
                {
                    Id = reader.GetInt32("id"),
                    Tipo = reader.GetString("tipo")
                };
            }
            return null;
        }

        public List<TipoTelefoneModel> ListarTodos()
        {
            var lista = new List<TipoTelefoneModel>();

            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "SELECT * FROM TipoTelefone";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new TipoTelefoneModel
                {
                    Id = reader.GetInt32("id"),
                    Tipo = reader.GetString("tipo")
                });
            }

            return lista;
        }

        public int Inserir(TipoTelefoneModel obj)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "INSERT INTO TipoTelefone (tipo) VALUES (@tipo); SELECT LAST_INSERT_ID()";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@tipo", obj.Tipo);

            int idGerado = Convert.ToInt32(cmd.ExecuteScalar());
            return idGerado;
        }

        public void Atualizar(TipoTelefoneModel obj)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "UPDATE TipoTelefone SET tipo = @tipo WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@tipo", obj.Tipo);
            cmd.Parameters.AddWithValue("@id", obj.Id);

            cmd.ExecuteNonQuery();
        }

        public void Remover(int id)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "DELETE FROM TipoTelefone WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
