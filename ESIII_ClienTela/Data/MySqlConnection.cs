using DotNetEnv;
using MySql.Data.MySqlClient;

namespace ESIII_ClienTela.Data
{
    public class MySqlConnectionDB
    {
        private static string connectionString;

        static MySqlConnectionDB()
        {
            Env.Load(); // carrega o .env

            connectionString = $"Server={Env.GetString("DB_HOST")};" +
                               $"Port={Env.GetString("DB_PORT")};" +
                               $"Database={Env.GetString("DB_NAME")};" +
                               $"Uid={Env.GetString("DB_USER")};" +
                               $"Pwd={Env.GetString("DB_PASS")};";
        }

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
