using Npgsql;

namespace library_system
{
    public class Database
    {
        private readonly string connectionString =
            Environment.GetEnvironmentVariable("LIBRARY_DB_CONNECTION") ??
            "Host=localhost;" +
            "Port=5432;" +
            "Database=library_management;" +
            "Username=postgres;" +
            "Password=1234";

        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
