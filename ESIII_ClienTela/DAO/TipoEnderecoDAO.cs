using MySql.Data.MySqlClient;
using ESIII_ClienTela.Data;
using ESIII_ClienTela.Models;

namespace ESIII_ClienTela.DAO
{
    public class TipoEnderecoDAO : IDAO<TipoEnderecoModel>
    {
        public TipoEnderecoModel ObterPorId(int id)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "SELECT * FROM TipoEndereco WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new TipoEnderecoModel
                {
                    Id = reader.GetInt32("id"),
                    Tipo = reader.GetString("tipo")
                };
            }
            return null;
        }

        public List<TipoEnderecoModel> ListarTodos()
        {
            var lista = new List<TipoEnderecoModel>();

            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "SELECT * FROM TipoEndereco";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new TipoEnderecoModel
                {
                    Id = reader.GetInt32("id"),
                    Tipo = reader.GetString("tipo")
                });
            }

            return lista;
        }

        public void Inserir(TipoEnderecoModel obj)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "INSERT INTO TipoEndereco (tipo) VALUES (@tipo)";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@tipo", obj.Tipo);

            cmd.ExecuteNonQuery();
        }

        public void Atualizar(TipoEnderecoModel obj)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "UPDATE TipoEndereco SET tipo = @tipo WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@tipo", obj.Tipo);
            cmd.Parameters.AddWithValue("@id", obj.Id);

            cmd.ExecuteNonQuery();
        }

        public void Remover(int id)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "DELETE FROM TipoEndereco WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}

