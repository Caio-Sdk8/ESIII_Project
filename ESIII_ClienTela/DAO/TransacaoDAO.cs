using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ESIII_ClienTela.Data;
using ESIII_ClienTela.Models;

namespace ESIII_ClienTela.DAO
{
    public class TransacaoDAO : IDAO<TransacaoModel>
    {
        public TransacaoModel ObterPorId(int id)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "SELECT * FROM Transacao WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new TransacaoModel
                {
                    Id = reader.GetInt32("id"),
                    Cliente_id = reader.GetInt32("cliente_id"),
                    Data = reader.GetDateTime("data"),
                    Valor = reader.GetFloat("valor")
                };
            }
            return null;
        }

        public List<TransacaoModel> ListarTodos()
        {
            var lista = new List<TransacaoModel>();

            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "SELECT * FROM Transacao";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new TransacaoModel
                {
                    Id = reader.GetInt32("id"),
                    Cliente_id = reader.GetInt32("cliente_id"),
                    Data = reader.GetDateTime("data"),
                    Valor = reader.GetFloat("valor")
                });
            }

            return lista;
        }

        public void Inserir(TransacaoModel obj)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "INSERT INTO Transacao (cliente_id, data, valor) VALUES (@cliente_id, @data, @valor)";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@cliente_id", obj.Cliente_id);
            cmd.Parameters.AddWithValue("@data", obj.Data);
            cmd.Parameters.AddWithValue("@valor", obj.Valor);

            cmd.ExecuteNonQuery();
        }

        public void Atualizar(TransacaoModel obj)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "UPDATE Transacao SET cliente_id = @cliente_id, data = @data, valor = @valor WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@cliente_id", obj.Cliente_id);
            cmd.Parameters.AddWithValue("@data", obj.Data);
            cmd.Parameters.AddWithValue("@valor", obj.Valor);
            cmd.Parameters.AddWithValue("@id", obj.Id);

            cmd.ExecuteNonQuery();
        }

        public void Remover(int id)
        {
            using var conn = MySqlConnectionDB.GetConnection();
            conn.Open();

            string sql = "DELETE FROM Transacao WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
