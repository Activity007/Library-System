using Npgsql;

namespace library_system;

internal static class UiAction
{
    public static bool Run(IWin32Window owner, Action action)
    {
        try { action(); return true; }
        catch (Exception ex)
        {
            string message = ex switch
            {
                PostgresException { SqlState: "23503" } => "This record is used by another record. Keep it to preserve library history, and check your selected category, member, and book.",
                PostgresException { SqlState: "23505" } => "A record with the same unique value already exists.",
                PostgresException { SqlState: "23514" } => "The change would violate library rules. Check quantities, dates, and status.",
                PostgresException { SqlState: "42P01" } => "The database tables are missing. Run database/schema.sql before using the library.",
                NpgsqlException => "Cannot access the library database. Check that PostgreSQL is running and the connection settings are correct.",
                _ => ex.Message
            };
            MessageBox.Show(owner, message, "Library Management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
    }

    public static bool Confirm(IWin32Window owner, string message) =>
        MessageBox.Show(owner, message, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
}
