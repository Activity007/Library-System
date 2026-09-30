namespace library_system;

public sealed class LoanRecord
{
    public int BorrowID { get; init; }
    public int BorrowDetailID { get; init; }
    public int BookID { get; init; }
    public string MemberName { get; init; } = "";
    public string BookTitle { get; init; } = "";
    public string LibrarianName { get; init; } = "";
    public DateTime BorrowDate { get; init; }
    public DateTime DueDate { get; init; }
    public DateTime? ReturnDate { get; init; }
    public decimal Fine { get; init; }
    public string Status => ReturnDate.HasValue ? "Returned" : DueDate.Date < DateTime.Today ? "Overdue" : "Borrowed";
}
