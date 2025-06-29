using MySql.Data.MySqlClient;
using ESIII_ClienTela.Data;
using ESIII_ClienTela.Models;

namespace ESIII_ClienTela.DAO
{
    public class EstadoDAO : IDAO<EstadoModel>
    {
        public EstadoModel ObterPorId(int id)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "SELECT * FROM Estado WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new EstadoModel
                {
                    Id = reader.GetInt32("id"),
                    Nome = reader.GetString("nome"),
                    Uf = reader.GetString("uf"),
                    Pais_id = reader.GetInt32("pais_id")
                };
            }
            return null;
        }

        public List<EstadoModel> ListarTodos()
        {
            var lista = new List<EstadoModel>();

            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "SELECT * FROM Estado";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new EstadoModel
                {
                    Id = reader.GetInt32("id"),
                    Nome = reader.GetString("nome"),
                    Uf = reader.GetString("uf"),
                    Pais_id = reader.GetInt32("pais_id")
                });
            }

            return lista;
        }

        public void Inserir(EstadoModel estado)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "INSERT INTO Estado (nome, uf, pais_id) VALUES (@nome, @uf, @pais_id)";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@nome", estado.Nome);
            cmd.Parameters.AddWithValue("@uf", estado.Uf);
            cmd.Parameters.AddWithValue("@pais_id", estado.Pais_id);

            cmd.ExecuteNonQuery();
        }

        public void Atualizar(EstadoModel estado)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "UPDATE Estado SET nome = @nome, uf = @uf, pais_id = @pais_id WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@nome", estado.Nome);
            cmd.Parameters.AddWithValue("@uf", estado.Uf);
            cmd.Parameters.AddWithValue("@pais_id", estado.Pais_id);
            cmd.Parameters.AddWithValue("@id", estado.Id);

            cmd.ExecuteNonQuery();
        }

        public void Remover(int id)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "DELETE FROM Estado WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}


