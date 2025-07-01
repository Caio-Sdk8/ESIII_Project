using ESIII_ClienTela.Data;
using ESIII_ClienTela.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

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

        public int Inserir(ClienteModel cliente)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = @"INSERT INTO Cliente 
                            (nome, genero, dataNascimento, cpf, email, senha, status, ranking) 
                           VALUES 
                            (@nome, @genero, @dataNascimento, @cpf, @email, @senha, @status, @ranking);
                            SELECT LAST_INSERT_ID()";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@nome", cliente.Nome);
            cmd.Parameters.AddWithValue("@genero", cliente.Genero);
            cmd.Parameters.AddWithValue("@dataNascimento", cliente.DataNascimento.ToDateTime(new TimeOnly(0, 0)));
            cmd.Parameters.AddWithValue("@cpf", cliente.Cpf);
            cmd.Parameters.AddWithValue("@email", cliente.Email);
            cmd.Parameters.AddWithValue("@senha", cliente.Senha);
            cmd.Parameters.AddWithValue("@status", cliente.Status);
            cmd.Parameters.AddWithValue("@ranking", cliente.Ranking);

            int idGerado = Convert.ToInt32(cmd.ExecuteScalar());
            return idGerado;
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

        public List<ClienteModel> BuscarClientesParams(string? nome, string? email, string? telefone, string? cpf)
        {
            var clientes = new List<ClienteModel>();

            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            var query = @"
                SELECT c.*
                FROM Cliente c
                LEFT JOIN Telefone t ON c.id = t.clienteId
                WHERE 
                    (@nome IS NULL OR c.nome LIKE @nome) AND
                    (@email IS NULL OR c.email LIKE @email) AND
                    (@telefone IS NULL OR t.numero LIKE @telefone)
                    (@cpf IS NULL OR t.cpf LIKE @cpf)
            ";

            using var command = new MySqlCommand(query, conn);

            command.Parameters.AddWithValue("@nome", string.IsNullOrWhiteSpace(nome) ? DBNull.Value : $"%{nome}%");
            command.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(email) ? DBNull.Value : $"%{email}%");
            command.Parameters.AddWithValue("@telefone", string.IsNullOrWhiteSpace(telefone) ? DBNull.Value : $"%{telefone}%");
            command.Parameters.AddWithValue("@cpf", string.IsNullOrWhiteSpace(cpf) ? DBNull.Value : $"%{cpf}%");

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var cliente = new ClienteModel
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

                clientes.Add(cliente);
            }

            return clientes;
        }

        public bool ExisteRanking(int ranking)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "SELECT COUNT(*) FROM Cliente WHERE ranking = @ranking";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ranking", ranking);

            long count = (long)cmd.ExecuteScalar();
            return count > 0;
        }
    }
}
