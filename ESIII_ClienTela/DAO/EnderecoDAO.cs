using MySql.Data.MySqlClient;
using ESIII_ClienTela.Data;
using ESIII_ClienTela.Models;

namespace ESIII_ClienTela.DAO
{
    public class EnderecoDAO : IDAO<EnderecoModel>
    {
        public EnderecoModel ObterPorId(int id)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "SELECT * FROM Endereco WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new EnderecoModel
                {
                    Id = reader.GetInt32("id"),
                    Cliente_id = reader.GetInt32("cliente_id"),
                    Cidade_id = reader.GetInt32("cidade_id"),
                    TipoLogradouro_id = reader.GetInt32("tipoLogradouro_id"),
                    TipoResidencia_id = reader.GetInt32("tipoResidencia_id"),
                    TipoEndereco_id = reader.GetInt32("tipoEndereco_id"),
                    Apelido = reader.GetString("apelido"),
                    Logradouro = reader.GetString("logradouro"),
                    Numero = reader.GetString("numero"),
                    Bairro = reader.GetString("bairro"),
                    Cep = reader.GetString("cep"),
                    Obs = reader.GetString("obs")
                };
            }
            return null;
        }

        public List<EnderecoModel> ListarTodos()
        {
            var lista = new List<EnderecoModel>();

            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "SELECT * FROM Endereco";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new EnderecoModel
                {
                    Id = reader.GetInt32("id"),
                    Cliente_id = reader.GetInt32("cliente_id"),
                    Cidade_id = reader.GetInt32("cidade_id"),
                    TipoLogradouro_id = reader.GetInt32("tipoLogradouro_id"),
                    TipoResidencia_id = reader.GetInt32("tipoResidencia_id"),
                    TipoEndereco_id = reader.GetInt32("tipoEndereco_id"),
                    Apelido = reader.GetString("apelido"),
                    Logradouro = reader.GetString("logradouro"),
                    Numero = reader.GetString("numero"),
                    Bairro = reader.GetString("bairro"),
                    Cep = reader.GetString("cep"),
                    Obs = reader.GetString("obs")
                });
            }

            return lista;
        }

        public int Inserir(EnderecoModel endereco)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = @"
                INSERT INTO Endereco 
                (cliente_id, cidade_id, tipoLogradouro_id, tipoResidencia_id, tipoEndereco_id, apelido, logradouro, numero, bairro, cep, obs) 
                VALUES 
                (@cliente_id, @cidade_id, @tipoLogradouro_id, @tipoResidencia_id, @tipoEndereco_id, @apelido, @logradouro, @numero, @bairro, @cep, @obs);
                SELECT LAST_INSERT_ID()";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@cliente_id", endereco.Cliente_id);
            cmd.Parameters.AddWithValue("@cidade_id", endereco.Cidade_id);
            cmd.Parameters.AddWithValue("@tipoLogradouro_id", endereco.TipoLogradouro_id);
            cmd.Parameters.AddWithValue("@tipoResidencia_id", endereco.TipoResidencia_id);
            cmd.Parameters.AddWithValue("@tipoEndereco_id", endereco.TipoEndereco_id);
            cmd.Parameters.AddWithValue("@apelido", endereco.Apelido);
            cmd.Parameters.AddWithValue("@logradouro", endereco.Logradouro);
            cmd.Parameters.AddWithValue("@numero", endereco.Numero);
            cmd.Parameters.AddWithValue("@bairro", endereco.Bairro);
            cmd.Parameters.AddWithValue("@cep", endereco.Cep);
            cmd.Parameters.AddWithValue("@obs", endereco.Obs);

            int idGerado = Convert.ToInt32(cmd.ExecuteScalar());
            return idGerado;
        }

        public void Atualizar(EnderecoModel endereco)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = @"
                UPDATE Endereco SET 
                    cliente_id = @cliente_id,
                    cidade_id = @cidade_id,
                    tipoLogradouro_id = @tipoLogradouro_id,
                    tipoResidencia_id = @tipoResidencia_id,
                    tipoEndereco_id = @tipoEndereco_id,
                    apelido = @apelido,
                    logradouro = @logradouro,
                    numero = @numero,
                    bairro = @bairro,
                    cep = @cep,
                    obs = @obs
                WHERE id = @id";

            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@cliente_id", endereco.Cliente_id);
            cmd.Parameters.AddWithValue("@cidade_id", endereco.Cidade_id);
            cmd.Parameters.AddWithValue("@tipoLogradouro_id", endereco.TipoLogradouro_id);
            cmd.Parameters.AddWithValue("@tipoResidencia_id", endereco.TipoResidencia_id);
            cmd.Parameters.AddWithValue("@tipoEndereco_id", endereco.TipoEndereco_id);
            cmd.Parameters.AddWithValue("@apelido", endereco.Apelido);
            cmd.Parameters.AddWithValue("@logradouro", endereco.Logradouro);
            cmd.Parameters.AddWithValue("@numero", endereco.Numero);
            cmd.Parameters.AddWithValue("@bairro", endereco.Bairro);
            cmd.Parameters.AddWithValue("@cep", endereco.Cep);
            cmd.Parameters.AddWithValue("@obs", endereco.Obs);
            cmd.Parameters.AddWithValue("@id", endereco.Id);

            cmd.ExecuteNonQuery();
        }

        public void Remover(int id)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "DELETE FROM Endereco WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }

        public List<EnderecoModel> BuscarPorClienteId(int clienteId)
        {
            var enderecos = new List<EnderecoModel>();

            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string query = "SELECT * FROM Endereco WHERE clienteId = @clienteId";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@clienteId", clienteId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var endereco = new EnderecoModel
                {
                    Id = reader.GetInt32("id"),
                    Apelido = reader.GetString("apelido"),
                    Cliente_id = reader.GetInt32("clienteId"),
                    Logradouro = reader.GetString("logradouro"),
                    Numero = reader.GetString("numero"),
                    Bairro = reader.GetString("bairro"),
                    Cep = reader.GetString("cep"),
                    Cidade_id = reader.GetInt32("cidadeId"),
                    TipoEndereco_id = reader.GetInt32("tipoEnderecoId"),
                    TipoLogradouro_id = reader.GetInt32("tipoLogradouroId"),
                    TipoResidencia_id = reader.GetInt32("tiporesidenciaId"),
                    Obs = reader.GetString("obs"),

                };

                enderecos.Add(endereco);
            }

            return enderecos;
        }
    }

}
