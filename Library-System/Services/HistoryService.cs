using Npgsql;

namespace library_system;

public sealed class HistoryService
{
    public List<LoanRecord> GetLoans(string keyword = "", string status = "All",
        DateTime? from = null, DateTime? to = null, int? borrowID = null)
    {
        using var connection = new Database().GetConnection();
        connection.Open();
        using var command = new NpgsqlCommand("""
            SELECT b.BorrowID, d.BorrowDetailID, k.BookID, m.Name, k.Title,
                   l.Name AS LibrarianName, b.BorrowDate, b.DueDate, d.ReturnDate, d.Fine
            FROM Borrow b
            JOIN BorrowDetail d ON d.BorrowID = b.BorrowID
            JOIN Book k ON k.BookID = d.BookID
            JOIN Member m ON m.MemberID = b.MemberID
            JOIN Librarian l ON l.LibrarianID = b.LibrarianID
            WHERE (@id = 0 OR b.BorrowID = @id)
              AND b.BorrowDate BETWEEN @from AND @to
              AND (m.Name ILIKE @keyword OR k.Title ILIKE @keyword
                   OR b.BorrowID::text ILIKE @keyword OR l.Name ILIKE @keyword)
              AND (@status = 'All'
                   OR (@status = 'Returned' AND d.ReturnDate IS NOT NULL)
                   OR (@status = 'Borrowed' AND d.ReturnDate IS NULL AND b.DueDate >= CURRENT_DATE)
                   OR (@status = 'Overdue' AND d.ReturnDate IS NULL AND b.DueDate < CURRENT_DATE))
            ORDER BY b.BorrowDate DESC, b.BorrowID DESC, d.BorrowDetailID
            """, connection);
        command.Parameters.AddWithValue("id", borrowID ?? 0);
        command.Parameters.AddWithValue("from", NpgsqlTypes.NpgsqlDbType.Date, (from ?? DateTime.MinValue).Date);
        command.Parameters.AddWithValue("to", NpgsqlTypes.NpgsqlDbType.Date, (to ?? DateTime.MaxValue).Date);
        command.Parameters.AddWithValue("keyword", "%" + keyword.Trim() + "%");
        command.Parameters.AddWithValue("status", status);
        using var reader = command.ExecuteReader();
        var loans = new List<LoanRecord>();
        while (reader.Read())
            loans.Add(new LoanRecord
            {
                BorrowID = reader.GetInt32(0), BorrowDetailID = reader.GetInt32(1),
                BookID = reader.GetInt32(2), MemberName = reader.GetString(3),
                BookTitle = reader.GetString(4), LibrarianName = reader.GetString(5),
                BorrowDate = reader.GetDateTime(6), DueDate = reader.GetDateTime(7),
                ReturnDate = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                Fine = reader.GetDecimal(9)
            });
        return loans;
    }

    public Dictionary<string, string> GetDashboard()
    {
        using var connection = new Database().GetConnection();
        connection.Open();
        using var command = new NpgsqlCommand("""
            SELECT (SELECT COUNT(*) FROM Book),
                   (SELECT COALESCE(SUM(AvailableQuantity), 0) FROM Book),
                   (SELECT COUNT(*) FROM Category),
                   (SELECT COUNT(*) FROM Member WHERE Status = 'Active'),
                   (SELECT COUNT(*) FROM BorrowDetail WHERE ReturnDate IS NULL),
                   (SELECT COUNT(*) FROM BorrowDetail d JOIN Borrow b USING (BorrowID)
                    WHERE d.ReturnDate IS NULL AND b.DueDate < CURRENT_DATE),
                   (SELECT COUNT(*) FROM Borrow),
                   (SELECT COUNT(*) FROM BorrowDetail WHERE ReturnDate IS NOT NULL)
            """, connection);
        using var reader = command.ExecuteReader();
        reader.Read();
        return new()
        {
            ["Books"] = $"{reader.GetInt64(0)} titles · {reader.GetInt64(1)} available copies",
            ["Categories"] = $"{reader.GetInt64(2)} categories",
            ["Members"] = $"{reader.GetInt64(3)} active members",
            ["Borrow"] = $"{reader.GetInt64(4)} copies on loan",
            ["Return"] = $"{reader.GetInt64(5)} overdue · {reader.GetInt64(7)} returned",
            ["History"] = $"{reader.GetInt64(6)} borrowing transactions"
        };
    }
}
