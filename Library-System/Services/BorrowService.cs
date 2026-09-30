using Npgsql;
using NpgsqlTypes;

namespace library_system;

public class BorrowService
{
    public List<Book> GetAvailableBooks() => new BookService().GetAllBooks()
        .Where(book => book.AvailableQuantity > 0).OrderBy(book => book.Title).ToList();

    public List<Member> GetActiveMembers() => new MemberService().GetAllMembers()
        .Where(member => member.Status == "Active").OrderBy(member => member.Name).ToList();

    public int BorrowBooks(int memberID, int librarianID, DateTime borrowDate, DateTime dueDate, List<int> bookIDs)
    {
        LoanRules.ValidateBorrow(borrowDate, dueDate, bookIDs);
        using var connection = new Database().GetConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();
        using (var member = new NpgsqlCommand(
            "SELECT Status FROM Member WHERE MemberID = @id FOR UPDATE", connection, transaction))
        {
            member.Parameters.AddWithValue("id", memberID);
            if (member.ExecuteScalar() as string != "Active")
                throw new InvalidOperationException("Select an active member.");
        }

        using var borrow = new NpgsqlCommand("""
            INSERT INTO Borrow (MemberID, LibrarianID, BorrowDate, DueDate, Status)
            VALUES (@member, @librarian, @borrow, @due, 'Borrowed') RETURNING BorrowID
            """, connection, transaction);
        borrow.Parameters.AddWithValue("member", memberID);
        borrow.Parameters.AddWithValue("librarian", librarianID);
        borrow.Parameters.AddWithValue("borrow", NpgsqlDbType.Date, borrowDate.Date);
        borrow.Parameters.AddWithValue("due", NpgsqlDbType.Date, dueDate.Date);
        int borrowID = Convert.ToInt32(borrow.ExecuteScalar());

        // Stable lock order and a conditional update prevent overselling the last copy.
        foreach (int bookID in bookIDs.Order())
        {
            using var stock = new NpgsqlCommand("""
                UPDATE Book SET AvailableQuantity = AvailableQuantity - 1
                WHERE BookID = @id AND AvailableQuantity > 0
                """, connection, transaction);
            stock.Parameters.AddWithValue("id", bookID);
            if (stock.ExecuteNonQuery() != 1)
                throw new InvalidOperationException("A selected book is no longer available. Refresh and try again.");

            using var detail = new NpgsqlCommand("""
                INSERT INTO BorrowDetail (BorrowID, BookID, Fine, Status)
                VALUES (@borrow, @book, 0, 'Borrowed')
                """, connection, transaction);
            detail.Parameters.AddWithValue("borrow", borrowID);
            detail.Parameters.AddWithValue("book", bookID);
            detail.ExecuteNonQuery();
        }
        transaction.Commit();
        return borrowID;
    }
}
