using MySql.Data.MySqlClient;
using ESIII_ClienTela.Data;
using ESIII_ClienTela.Models;

namespace ESIII_ClienTela.DAO
{
    public class TipoLogradouroDAO : IDAO<TipoLogradouroModel>
    {
        public TipoLogradouroModel ObterPorId(int id)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "SELECT * FROM TipoLogradouro WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new TipoLogradouroModel
                {
                    Id = reader.GetInt32("id"),
                    Tipo = reader.GetString("tipo")
                };
            }
            return null;
        }

        public List<TipoLogradouroModel> ListarTodos()
        {
            var lista = new List<TipoLogradouroModel>();

            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "SELECT * FROM TipoLogradouro";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new TipoLogradouroModel
                {
                    Id = reader.GetInt32("id"),
                    Tipo = reader.GetString("tipo")
                });
            }

            return lista;
        }

        public void Inserir(TipoLogradouroModel obj)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "INSERT INTO TipoLogradouro (tipo) VALUES (@tipo)";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@tipo", obj.Tipo);

            cmd.ExecuteNonQuery();
        }

        public void Atualizar(TipoLogradouroModel obj)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "UPDATE TipoLogradouro SET tipo = @tipo WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@tipo", obj.Tipo);
            cmd.Parameters.AddWithValue("@id", obj.Id);

            cmd.ExecuteNonQuery();
        }

        public void Remover(int id)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "DELETE FROM TipoLogradouro WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}