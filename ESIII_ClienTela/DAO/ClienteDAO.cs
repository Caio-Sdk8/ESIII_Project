using ESIII_ClienTela.Data;
using ESIII_ClienTela.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.Data;

namespace ESIII_ClienTela.DAO
{
    public class ClienteDAO : IDAO<ClienteModel>
    {
        public ClienteModel ObterPorId(int id)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = @"
                SELECT 
                    c.id AS clienteId, c.nome, c.genero, c.dataNascimento, c.cpf, c.email, c.senha, c.status, c.ranking,

                    t.id AS telefoneId, t.ddd AS telefoneDdd, t.numero AS telefoneNumero, t.tipoTelefoneId,

                    e.id AS enderecoId, e.apelido AS enderecoApelido, e.logradouro AS enderecoLogradouro, 
                    e.numero AS enderecoNumero, e.bairro AS enderecoBairro, e.cep AS enderecoCep, e.obs AS enderecoObs,
                    e.cidadeId, e.tipoLogradouroId, e.tipoResidenciaId, e.tipoEnderecoId,

                    ct.id AS cartaoId, ct.numero AS cartaoNumero, ct.nomeImpresso, ct.codSeguranca, 
                    ct.band AS cartaoBandeira, ct.preferencial

                FROM Cliente c
                LEFT JOIN Telefone t ON t.clienteId = c.id
                LEFT JOIN Endereco e ON e.clienteId = c.id
                LEFT JOIN CartaoDeCredito ct ON ct.clienteId = c.id
                WHERE c.id = @id
                ORDER BY c.id;
            ";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            ClienteModel cliente = null;

            while (reader.Read())
            {
                if (cliente == null)
                {
                    cliente = new ClienteModel
                    {
                        Id = reader.GetInt32("clienteId"),
                        Nome = reader.GetString("nome"),
                        Genero = reader.GetString("genero"),
                        DataNascimento = reader.GetDateTime("dataNascimento"),
                        Cpf = reader.GetString("cpf"),
                        Email = reader.GetString("email"),
                        Senha = reader.GetString("senha"),
                        Status = reader.GetBoolean("status"),
                        Ranking = reader.GetInt32("ranking"),
                        Telefones = new(),
                        Enderecos = new(),
                        Cartoes = new()
                    };
                }

                if (!reader.IsDBNull(reader.GetOrdinal("telefoneId")))
                {
                    int telId = reader.GetInt32("telefoneId");
                    if (!cliente.Telefones.Any(t => t.Id == telId))
                    {
                        cliente.Telefones.Add(new TelefoneModel
                        {
                            Id = telId,
                            Cliente_id = cliente.Id,
                            Ddd = reader.GetString("telefoneDdd"),
                            Numero = reader.GetString("telefoneNumero"),
                            TipoTelefone_id = reader.IsDBNull("tipoTelefoneId") ? 0 : reader.GetInt32("tipoTelefoneId")
                        });
                    }
                }

                if (!reader.IsDBNull(reader.GetOrdinal("enderecoId")))
                {
                    int endId = reader.GetInt32("enderecoId");
                    if (!cliente.Enderecos.Any(e => e.Id == endId))
                    {
                        cliente.Enderecos.Add(new EnderecoModel
                        {
                            Id = endId,
                            Cliente_id = cliente.Id,
                            Apelido = reader.GetString("enderecoApelido"),
                            Logradouro = reader.GetString("enderecoLogradouro"),
                            Numero = reader.GetString("enderecoNumero"),
                            Bairro = reader.GetString("enderecoBairro"),
                            Cep = reader.GetString("enderecoCep"),
                            Obs = reader.GetString("enderecoObs"),
                            Cidade_id = reader.GetInt32("cidadeId"),
                            TipoLogradouro_id = reader.GetInt32("tipoLogradouroId"),
                            TipoResidencia_id = reader.GetInt32("tipoResidenciaId"),
                            TipoEndereco_id = reader.GetInt32("tipoEnderecoId")
                        });
                    }
                }

                if (!reader.IsDBNull(reader.GetOrdinal("cartaoId")))
                {
                    int cartaoId = reader.GetInt32("cartaoId");
                    if (!cliente.Cartoes.Any(c => c.Id == cartaoId))
                    {
                        cliente.Cartoes.Add(new CartaoDeCreditoModel
                        {
                            Id = cartaoId,
                            Cliente_id = cliente.Id,
                            Numero = reader.GetString("cartaoNumero"),
                            NomeImpresso = reader.GetString("nomeImpresso"),
                            CodSeguranca = reader.GetString("codSeguranca"),
                            Band = reader.GetString("cartaoBandeira"),
                            Preferencial = reader.IsDBNull("preferencial") ? false : reader.GetBoolean("preferencial")
                        });
                    }
                }
            }

            return cliente;
        }
        public List<ClienteModel> ListarTodos()
        {
            var clientes = new Dictionary<int, ClienteModel>();

            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = @"
                SELECT 
                    c.id AS clienteId, c.nome, c.genero, c.dataNascimento, c.cpf, c.email, c.senha, c.status, c.ranking,

                    t.id AS telefoneId, t.ddd AS telefoneDdd, t.numero AS telefoneNumero, t.tipoTelefoneId,

                    e.id AS enderecoId, e.apelido AS enderecoApelido, e.logradouro AS enderecoLogradouro, 
                    e.numero AS enderecoNumero, e.bairro AS enderecoBairro, e.cep AS enderecoCep, e.obs AS enderecoObs,
                    e.cidadeId, e.tipoLogradouroId, e.tipoResidenciaId, e.tipoEnderecoId,

                    ct.id AS cartaoId, ct.numero AS cartaoNumero, ct.nomeImpresso, ct.codSeguranca, 
                    ct.band AS cartaoBandeira, ct.preferencial

                FROM Cliente c
                LEFT JOIN Telefone t ON t.clienteId = c.id
                LEFT JOIN Endereco e ON e.clienteId = c.id
                LEFT JOIN CartaoDeCredito ct ON ct.clienteId = c.id
                ORDER BY c.id;
            ";

            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int clienteId = reader.GetInt32("clienteId");

                if (!clientes.TryGetValue(clienteId, out var cliente))
                {
                    cliente = new ClienteModel
                    {
                        Id = clienteId,
                        Nome = reader.GetString("nome"),
                        Genero = reader.GetString("genero"),
                        DataNascimento = reader.GetDateTime("dataNascimento"),
                        Cpf = reader.GetString("cpf"),
                        Email = reader.GetString("email"),
                        Senha = reader.GetString("senha"),
                        Status = reader.GetBoolean("status"),
                        Ranking = reader.GetInt32("ranking"),
                        Telefones = new(),
                        Enderecos = new(),
                        Cartoes = new()
                    };

                    clientes[clienteId] = cliente;
                }

                if (!reader.IsDBNull(reader.GetOrdinal("telefoneId")))
                {
                    cliente.Telefones.Add(new TelefoneModel
                    {
                        Id = reader.GetInt32("telefoneId"),
                        Cliente_id = clienteId,
                        Ddd = reader.GetString("telefoneDdd"),
                        Numero = reader.GetString("telefoneNumero"),
                        TipoTelefone_id = reader.IsDBNull(reader.GetOrdinal("tipoTelefoneId")) ? 0 : reader.GetInt32("tipoTelefoneId")
                    });
                }

                if (!reader.IsDBNull(reader.GetOrdinal("enderecoId")))
                {
                    cliente.Enderecos.Add(new EnderecoModel
                    {
                        Id = reader.GetInt32("enderecoId"),
                        Cliente_id = clienteId,
                        Apelido = reader.GetString("enderecoApelido"),
                        Logradouro = reader.GetString("enderecoLogradouro"),
                        Numero = reader.GetString("enderecoNumero"),
                        Bairro = reader.GetString("enderecoBairro"),
                        Cep = reader.GetString("enderecoCep"),
                        Obs = reader.GetString("enderecoObs"),
                        Cidade_id = reader.GetInt32("cidadeId"),
                        TipoLogradouro_id = reader.GetInt32("tipoLogradouroId"),
                        TipoResidencia_id = reader.GetInt32("tipoResidenciaId"),
                        TipoEndereco_id = reader.GetInt32("tipoEnderecoId"),
                    });
                }

                if (!reader.IsDBNull(reader.GetOrdinal("cartaoId")))
                {
                    cliente.Cartoes.Add(new CartaoDeCreditoModel
                    {
                        Id = reader.GetInt32("cartaoId"),
                        Cliente_id = clienteId,
                        Numero = reader.GetString("cartaoNumero"),
                        NomeImpresso = reader.GetString("nomeImpresso"),
                        CodSeguranca = reader.GetString("codSeguranca"),
                        Band = reader.GetString("cartaoBandeira"),
                        Preferencial = reader.IsDBNull(reader.GetOrdinal("preferencial")) ? false : reader.GetBoolean("preferencial")
                    });
                }
            }

            return clientes.Values.ToList();
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
            cmd.Parameters.AddWithValue("@dataNascimento", cliente.DataNascimento);
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
            cmd.Parameters.AddWithValue("@dataNascimento", cliente.DataNascimento);
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
            var clientes = new Dictionary<int, ClienteModel>();

            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            var query = @"
                SELECT 
                    c.id AS clienteId, c.nome, c.genero, c.dataNascimento, c.cpf, c.email, c.senha, c.status, c.ranking,

                    t.id AS telefoneId, t.ddd AS telefoneDdd, t.numero AS telefoneNumero, t.tipoTelefoneId,

                    e.id AS enderecoId, e.apelido AS enderecoApelido, e.logradouro AS enderecoLogradouro, 
                    e.numero AS enderecoNumero, e.bairro AS enderecoBairro, e.cep AS enderecoCep, e.obs AS enderecoObs,
                    e.cidadeId, e.tipoLogradouroId, e.tipoResidenciaId, e.tipoEnderecoId,

                    ct.id AS cartaoId, ct.numero AS cartaoNumero, ct.nomeImpresso, ct.codSeguranca, 
                    ct.band AS cartaoBandeira, ct.preferencial

                FROM Cliente c
                LEFT JOIN Telefone t ON t.clienteId = c.id
                LEFT JOIN Endereco e ON e.clienteId = c.id
                LEFT JOIN CartaoDeCredito ct ON ct.clienteId = c.id
                WHERE 
                    (@nome IS NULL OR c.nome LIKE @nome) AND
                    (@email IS NULL OR c.email LIKE @email) AND
                    (@telefone IS NULL OR t.numero LIKE @telefone) AND
                    (@cpf IS NULL OR c.cpf LIKE @cpf)
                ORDER BY c.id;
            ";

            using var command = new MySqlCommand(query, conn);
            command.Parameters.AddWithValue("@nome", string.IsNullOrWhiteSpace(nome) ? DBNull.Value : $"%{nome}%");
            command.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(email) ? DBNull.Value : $"%{email}%");
            command.Parameters.AddWithValue("@telefone", string.IsNullOrWhiteSpace(telefone) ? DBNull.Value : $"%{telefone}%");
            command.Parameters.AddWithValue("@cpf", string.IsNullOrWhiteSpace(cpf) ? DBNull.Value : $"%{cpf}%");

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                int clienteId = reader.GetInt32("clienteId");

                if (!clientes.TryGetValue(clienteId, out var cliente))
                {
                    cliente = new ClienteModel
                    {
                        Id = clienteId,
                        Nome = reader.GetString("nome"),
                        Genero = reader.GetString("genero"),
                        DataNascimento = reader.GetDateTime("dataNascimento"),
                        Cpf = reader.GetString("cpf"),
                        Email = reader.GetString("email"),
                        Senha = reader.GetString("senha"),
                        Status = reader.GetBoolean("status"),
                        Ranking = reader.GetInt32("ranking"),
                        Telefones = new(),
                        Enderecos = new(),
                        Cartoes = new()
                    };

                    clientes[clienteId] = cliente;
                }

                if (!reader.IsDBNull(reader.GetOrdinal("telefoneId")))
                {
                    int telId = reader.GetInt32("telefoneId");
                    if (!cliente.Telefones.Any(t => t.Id == telId))
                    {
                        cliente.Telefones.Add(new TelefoneModel
                        {
                            Id = telId,
                            Cliente_id = clienteId,
                            Ddd = reader.GetString("telefoneDdd"),
                            Numero = reader.GetString("telefoneNumero"),
                            TipoTelefone_id = reader.IsDBNull("tipoTelefoneId") ? 0 : reader.GetInt32("tipoTelefoneId")
                        });
                    }
                }

                if (!reader.IsDBNull(reader.GetOrdinal("enderecoId")))
                {
                    int endId = reader.GetInt32("enderecoId");
                    if (!cliente.Enderecos.Any(e => e.Id == endId))
                    {
                        cliente.Enderecos.Add(new EnderecoModel
                        {
                            Id = endId,
                            Cliente_id = clienteId,
                            Apelido = reader.GetString("enderecoApelido"),
                            Logradouro = reader.GetString("enderecoLogradouro"),
                            Numero = reader.GetString("enderecoNumero"),
                            Bairro = reader.GetString("enderecoBairro"),
                            Cep = reader.GetString("enderecoCep"),
                            Obs = reader.GetString("enderecoObs"),
                            Cidade_id = reader.GetInt32("cidadeId"),
                            TipoLogradouro_id = reader.GetInt32("tipoLogradouroId"),
                            TipoResidencia_id = reader.GetInt32("tipoResidenciaId"),
                            TipoEndereco_id = reader.GetInt32("tipoEnderecoId")
                        });
                    }
                }

                if (!reader.IsDBNull(reader.GetOrdinal("cartaoId")))
                {
                    int cartaoId = reader.GetInt32("cartaoId");
                    if (!cliente.Cartoes.Any(c => c.Id == cartaoId))
                    {
                        cliente.Cartoes.Add(new CartaoDeCreditoModel
                        {
                            Id = cartaoId,
                            Cliente_id = clienteId,
                            Numero = reader.GetString("cartaoNumero"),
                            NomeImpresso = reader.GetString("nomeImpresso"),
                            CodSeguranca = reader.GetString("codSeguranca"),
                            Band = reader.GetString("cartaoBandeira"),
                            Preferencial = reader.IsDBNull("preferencial") ? false : reader.GetBoolean("preferencial")
                        });
                    }
                }
            }

            return clientes.Values.ToList();
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
        public bool AtualizarSenha(int clienteId, string senhaHash)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            var sql = "UPDATE Cliente SET senha = @senha WHERE id = @id";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@senha", senhaHash);
            cmd.Parameters.AddWithValue("@id", clienteId);

            int rowsAffected = cmd.ExecuteNonQuery();
            return rowsAffected > 0;
        }
    }
}
