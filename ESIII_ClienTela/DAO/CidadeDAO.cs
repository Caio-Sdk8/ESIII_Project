using MySql.Data.MySqlClient;
using ESIII_ClienTela.Data;
using ESIII_ClienTela.Models;

namespace ESIII_ClienTela.DAO
{
    public class CidadeDAO : IDAO<CidadeModel>
    {
        public CidadeModel ObterPorId(int id)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "SELECT * FROM Cidade WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new CidadeModel
                {
                    Id = reader.GetInt32("id"),
                    Nome = reader.GetString("nome"),
                    Estado_id = reader.GetInt32("estado_id")
                };
            }
            return null;
        }

        public List<CidadeModel> ListarTodos()
        {
            var lista = new List<CidadeModel>();

            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "SELECT * FROM Cidade";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new CidadeModel
                {
                    Id = reader.GetInt32("id"),
                    Nome = reader.GetString("nome"),
                    Estado_id = reader.GetInt32("estado_id")
                });
            }

            return lista;
        }

        public void Inserir(CidadeModel cidade)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "INSERT INTO Cidade (nome, estado_id) VALUES (@nome, @estado_id)";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@nome", cidade.Nome);
            cmd.Parameters.AddWithValue("@estado_id", cidade.Estado_id);

            cmd.ExecuteNonQuery();
        }

        public void Atualizar(CidadeModel cidade)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "UPDATE Cidade SET nome = @nome, estado_id = @estado_id WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@nome", cidade.Nome);
            cmd.Parameters.AddWithValue("@estado_id", cidade.Estado_id);
            cmd.Parameters.AddWithValue("@id", cidade.Id);

            cmd.ExecuteNonQuery();
        }

        public void Remover(int id)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "DELETE FROM Cidade WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}

