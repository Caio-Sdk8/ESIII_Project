using MySql.Data.MySqlClient;
using ESIII_ClienTela.Data;
using ESIII_ClienTela.Models;

namespace ESIII_ClienTela.DAO
{
    public class ClienteDAO : IDAO<ClienteModel>
    {
        public ClienteModel ObterPorId(int id)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "SELECT * FROM Cliente WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new ClienteModel
                {
                    Id = reader.GetInt32("id"),
                    Nome = reader.GetString("nome"),
                    Genero = reader.GetString("genero"),
                    DataNascimento = DateOnly.FromDateTime(reader.GetDateTime("dataNascimento")),
                    Cpf = reader.GetString("cpf"),
                    Email = reader.GetString("email"),
                    Senha = reader.GetString("senha"),
                    Status = reader.GetString("status"),
                    Ranking = reader.GetInt32("ranking")
                };
            }
            return null;
        }

        public List<ClienteModel> ListarTodos()
        {
            var lista = new List<ClienteModel>();
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "SELECT * FROM Cliente";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new ClienteModel
                {
                    Id = reader.GetInt32("id"),
                    Nome = reader.GetString("nome"),
                    Genero = reader.GetString("genero"),
                    DataNascimento = DateOnly.FromDateTime(reader.GetDateTime("dataNascimento")),
                    Cpf = reader.GetString("cpf"),
                    Email = reader.GetString("email"),
                    Senha = reader.GetString("senha"),
                    Status = reader.GetString("status"),
                    Ranking = reader.GetInt32("ranking")
                });
            }
            return lista;
        }

        public void Inserir(ClienteModel cliente)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = @"INSERT INTO Cliente 
                            (nome, genero, dataNascimento, cpf, email, senha, status, ranking) 
                           VALUES 
                            (@nome, @genero, @dataNascimento, @cpf, @email, @senha, @status, @ranking)";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@nome", cliente.Nome);
            cmd.Parameters.AddWithValue("@genero", cliente.Genero);
            cmd.Parameters.AddWithValue("@dataNascimento", cliente.DataNascimento.ToDateTime(new TimeOnly(0, 0)));
            cmd.Parameters.AddWithValue("@cpf", cliente.Cpf);
            cmd.Parameters.AddWithValue("@email", cliente.Email);
            cmd.Parameters.AddWithValue("@senha", cliente.Senha);
            cmd.Parameters.AddWithValue("@status", cliente.Status);
            cmd.Parameters.AddWithValue("@ranking", cliente.Ranking);

            cmd.ExecuteNonQuery();
        }

        public void Atualizar(ClienteModel cliente)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = @"UPDATE Cliente SET 
                            nome = @nome,
                            genero = @genero,
                            dataNascimento = @dataNascimento,
                            cpf = @cpf,
                            email = @email,
                            senha = @senha,
                            status = @status,
                            ranking = @ranking
                           WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@nome", cliente.Nome);
            cmd.Parameters.AddWithValue("@genero", cliente.Genero);
            cmd.Parameters.AddWithValue("@dataNascimento", cliente.DataNascimento.ToDateTime(new TimeOnly(0, 0)));
            cmd.Parameters.AddWithValue("@cpf", cliente.Cpf);
            cmd.Parameters.AddWithValue("@email", cliente.Email);
            cmd.Parameters.AddWithValue("@senha", cliente.Senha);
            cmd.Parameters.AddWithValue("@status", cliente.Status);
            cmd.Parameters.AddWithValue("@ranking", cliente.Ranking);
            cmd.Parameters.AddWithValue("@id", cliente.Id);

            cmd.ExecuteNonQuery();
        }

        public void Remover(int id)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "DELETE FROM Cliente WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
