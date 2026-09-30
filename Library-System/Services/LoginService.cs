using Npgsql;
using System.Security.Cryptography;

namespace library_system;

public sealed class LoginService
{
    public Librarian? Authenticate(string username, string password)
    {
        using var connection = new Database().GetConnection();
        connection.Open();
        using var command = new NpgsqlCommand(
            "SELECT LibrarianID, Name, Username, Password FROM Librarian WHERE Username = @username", connection);
        command.Parameters.AddWithValue("username", username.Trim());
        using var reader = command.ExecuteReader();
        if (!reader.Read() || !VerifyPassword(password, reader.GetString(3))) return null;
        return new Librarian { LibrarianID = reader.GetInt32(0), Name = reader.GetString(1), Username = reader.GetString(2) };
    }

    public static string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 210000, HashAlgorithmName.SHA256, 32);
        return $"pbkdf2$210000${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    public static bool VerifyPassword(string password, string stored)
    {
        var parts = stored.Split('$');
        if (parts.Length != 4 || parts[0] != "pbkdf2" ||
            !int.TryParse(parts[1], out int iterations) || iterations < 10000 || iterations > 1000000) return false;
        try
        {
            byte[] expected = Convert.FromBase64String(parts[3]);
            if (expected.Length != 32) return false;
            byte[] actual = Rfc2898DeriveBytes.Pbkdf2(password, Convert.FromBase64String(parts[2]), iterations, HashAlgorithmName.SHA256, 32);
            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }
        catch (FormatException) { return false; }
    }
}
