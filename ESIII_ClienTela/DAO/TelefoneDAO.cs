using MySql.Data.MySqlClient;
using ESIII_ClienTela.Data;
using ESIII_ClienTela.Models;

namespace ESIII_ClienTela.DAO
{
    public class TelefoneDAO : IDAO<TelefoneModel>
    {
        public TelefoneModel ObterPorId(int id)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "SELECT * FROM Telefone WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new TelefoneModel
                {
                    Id = reader.GetInt32("id"),
                    Cliente_id = reader.GetInt32("cliente_id"),
                    TipoTelefone_id = reader.GetInt32("tipoTelefone_id"),
                    Ddd = reader.GetString("ddd"),
                    Numero = reader.GetString("numero")
                };
            }
            return null;
        }

        public List<TelefoneModel> ListarTodos()
        {
            var lista = new List<TelefoneModel>();

            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "SELECT * FROM Telefone";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new TelefoneModel
                {
                    Id = reader.GetInt32("id"),
                    Cliente_id = reader.GetInt32("cliente_id"),
                    TipoTelefone_id = reader.GetInt32("tipoTelefone_id"),
                    Ddd = reader.GetString("ddd"),
                    Numero = reader.GetString("numero")
                });
            }

            return lista;
        }

        public int Inserir(TelefoneModel telefone)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "INSERT INTO Telefone (cliente_id, tipoTelefone_id, ddd, numero) VALUES (@cliente_id, @tipoTelefone_id, @ddd, @numero); SELECT LAST_INSERT_ID()";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@cliente_id", telefone.Cliente_id);
            cmd.Parameters.AddWithValue("@tipoTelefone_id", telefone.TipoTelefone_id);
            cmd.Parameters.AddWithValue("@ddd", telefone.Ddd);
            cmd.Parameters.AddWithValue("@numero", telefone.Numero);

            int idGerado = Convert.ToInt32(cmd.ExecuteScalar());
            return idGerado;
        }

        public void Atualizar(TelefoneModel telefone)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "UPDATE Telefone SET clienteId = @cliente_id, tipoTelefoneId = @tipoTelefone_id, ddd = @ddd, numero = @numero WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@cliente_id", telefone.Cliente_id);
            cmd.Parameters.AddWithValue("@tipoTelefone_id", telefone.TipoTelefone_id);
            cmd.Parameters.AddWithValue("@ddd", telefone.Ddd);
            cmd.Parameters.AddWithValue("@numero", telefone.Numero);
            cmd.Parameters.AddWithValue("@id", telefone.Id);

            cmd.ExecuteNonQuery();
        }

        public void Remover(int id)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "DELETE FROM Telefone WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
