using MySql.Data.MySqlClient;
using ESIII_ClienTela.Data;
using ESIII_ClienTela.Models;

namespace ESIII_ClienTela.DAO
{
    public class PaisDAO : IDAO<PaisModel>
    {
        public PaisModel ObterPorId(int id)
        {
            using var conexao = MySqlConnectionDB.GetConnection();
            conexao.Open();

            string sql = "SELECT * FROM Pais WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? Mapear(reader) : null;
        }

        public List<PaisModel> ListarTodos()
        {
            var lista = new List<PaisModel>();

            using var conexao = MySqlConnectionDB.GetConnection();
            conexao.Open();

            string sql = "SELECT * FROM Pais";
            using var cmd = new MySqlCommand(sql, conexao);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(Mapear(reader));
            }

            return lista;
        }

        public void Inserir(PaisModel pais)
        {
            using var conexao = MySqlConnectionDB.GetConnection();
            conexao.Open();

            string sql = "INSERT INTO Pais (nome) VALUES (@nome)";
            using var cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@nome", pais.Nome);
            cmd.ExecuteNonQuery();
        }

        public void Atualizar(PaisModel pais)
        {
            using var conexao = MySqlConnectionDB.GetConnection();
            conexao.Open();

            string sql = "UPDATE Pais SET nome = @nome WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@nome", pais.Nome);
            cmd.Parameters.AddWithValue("@id", pais.Id);
            cmd.ExecuteNonQuery();
        }

        public void Remover(int id)
        {
            using var conexao = MySqlConnectionDB.GetConnection();
            conexao.Open();

            string sql = "DELETE FROM Pais WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        private PaisModel Mapear(MySqlDataReader reader)
        {
            return new PaisModel
            {
                Id = reader.GetInt32("id"),
                Nome = reader.GetString("nome")
            };
        }
    }
}

