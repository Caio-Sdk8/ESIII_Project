using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ESIII_ClienTela.Data;
using ESIII_ClienTela.Models;

namespace ESIII_ClienTela.DAO
{
    public class CartaoDeCreditoDAO : IDAO<CartaoDeCreditoModel>
    {
        public CartaoDeCreditoModel ObterPorId(int id)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "SELECT * FROM CartaoDeCredito WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? Mapear(reader) : null;
        }

        public List<CartaoDeCreditoModel> ListarTodos()
        {
            var lista = new List<CartaoDeCreditoModel>();

            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "SELECT * FROM CartaoDeCredito";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(Mapear(reader));
            }

            return lista;
        }

        public int Inserir(CartaoDeCreditoModel c)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = @"
                INSERT INTO CartaoDeCredito 
                (cliente_id, numero, nomeImpresso, codSeguranca, band) 
                VALUES 
                (@cliente_id, @numero, @nomeImpresso, @codSeguranca, @band);
                SELECT LAST_INSERT_ID()";

            using var cmd = new MySqlCommand(sql, conn);
            PreencherParametros(cmd, c);

            int idGerado = Convert.ToInt32(cmd.ExecuteScalar());
            return idGerado;
        }

        public void Atualizar(CartaoDeCreditoModel c)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = @"
                UPDATE CartaoDeCredito SET 
                    cliente_id = @cliente_id,
                    numero = @numero,
                    nomeImpresso = @nomeImpresso,
                    codSeguranca = @codSeguranca,
                    band = @band
                WHERE id = @id";

            using var cmd = new MySqlCommand(sql, conn);
            PreencherParametros(cmd, c);
            cmd.Parameters.AddWithValue("@id", c.Id);
            cmd.ExecuteNonQuery();
        }

        public void Remover(int id)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "DELETE FROM CartaoDeCredito WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        private void PreencherParametros(MySqlCommand cmd, CartaoDeCreditoModel c)
        {
            cmd.Parameters.AddWithValue("@cliente_id", c.Cliente_id);
            cmd.Parameters.AddWithValue("@numero", c.Numero);
            cmd.Parameters.AddWithValue("@nomeImpresso", c.NomeImpresso);
            cmd.Parameters.AddWithValue("@codSeguranca", c.CodSeguranca);
            cmd.Parameters.AddWithValue("@band", c.Band);
        }

        private CartaoDeCreditoModel Mapear(MySqlDataReader r)
        {
            return new CartaoDeCreditoModel
            {
                Id = r.GetInt32("id"),
                Cliente_id = r.GetInt32("clienteId"),
                Numero = r.GetString("numero"),
                NomeImpresso = r.GetString("nomeImpresso"),
                CodSeguranca = r.GetString("codSeguranca"),
                Band = r.GetString("band")
            };
        }
    }
}

