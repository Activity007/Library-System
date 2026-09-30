using Npgsql;
using NpgsqlTypes;

namespace library_system;

public class ReturnService
{
    public decimal ReturnBook(int borrowDetailID, DateTime returnDate)
    {
        if (returnDate.Date > DateTime.Today)
            throw new InvalidOperationException("Return date cannot be in the future.");
        using var connection = new Database().GetConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        // Serialize returns for the same loan before locking its detail.
        using var parent = new NpgsqlCommand("""
            SELECT b.BorrowID FROM Borrow b
            JOIN BorrowDetail d ON d.BorrowID = b.BorrowID
            WHERE d.BorrowDetailID = @id FOR UPDATE OF b
            """, connection, transaction);
        parent.Parameters.AddWithValue("id", borrowDetailID);
        object? parentID = parent.ExecuteScalar();
        if (parentID == null) throw new InvalidOperationException("Borrow record not found.");
        int borrowID = Convert.ToInt32(parentID);
        int bookID;
        DateTime borrowDate, dueDate;
        using (var find = new NpgsqlCommand("""
            SELECT d.BookID, b.BorrowDate, b.DueDate
            FROM BorrowDetail d JOIN Borrow b ON b.BorrowID = d.BorrowID
            WHERE d.BorrowDetailID = @id AND d.ReturnDate IS NULL FOR UPDATE OF d
            """, connection, transaction))
        {
            find.Parameters.AddWithValue("id", borrowDetailID);
            using var reader = find.ExecuteReader();
            if (!reader.Read()) throw new InvalidOperationException("This book has already been returned.");
            bookID = reader.GetInt32(0);
            borrowDate = reader.GetDateTime(1);
            dueDate = reader.GetDateTime(2);
        }
        if (returnDate.Date < borrowDate.Date)
            throw new InvalidOperationException("Return date cannot be before the borrow date.");
        decimal fine = LoanRules.CalculateFine(dueDate, returnDate);
        using (var update = new NpgsqlCommand("""
            UPDATE BorrowDetail SET ReturnDate = @date, Fine = @fine, Status = 'Returned'
            WHERE BorrowDetailID = @id
            """, connection, transaction))
        {
            update.Parameters.AddWithValue("date", NpgsqlDbType.Date, returnDate.Date);
            update.Parameters.AddWithValue("fine", fine);
            update.Parameters.AddWithValue("id", borrowDetailID);
            update.ExecuteNonQuery();
        }
        using (var stock = new NpgsqlCommand(
            "UPDATE Book SET AvailableQuantity = AvailableQuantity + 1 WHERE BookID = @id", connection, transaction))
        {
            stock.Parameters.AddWithValue("id", bookID);
            stock.ExecuteNonQuery();
        }
        using (var status = new NpgsqlCommand("""
            UPDATE Borrow SET Status = CASE WHEN EXISTS (
                SELECT 1 FROM BorrowDetail WHERE BorrowID = @id AND ReturnDate IS NULL
            ) THEN 'Borrowed' ELSE 'Returned' END WHERE BorrowID = @id
            """, connection, transaction))
        {
            status.Parameters.AddWithValue("id", borrowID);
            status.ExecuteNonQuery();
        }
        transaction.Commit();
        return fine;
    }
}
