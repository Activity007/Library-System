namespace library_system;

public static class LoanRules
{
    public const decimal DailyFine = 1m;
    public static decimal CalculateFine(DateTime dueDate, DateTime returnDate) =>
        Math.Max(0, (returnDate.Date - dueDate.Date).Days) * DailyFine;

    public static void ValidateBorrow(DateTime borrowDate, DateTime dueDate, IReadOnlyCollection<int> bookIDs)
    {
        if (bookIDs.Count == 0) throw new InvalidOperationException("Add at least one book.");
        if (bookIDs.Distinct().Count() != bookIDs.Count)
            throw new InvalidOperationException("Each book can be selected only once per borrowing transaction.");
        if (borrowDate.Date > DateTime.Today)
            throw new InvalidOperationException("Borrow date cannot be in the future.");
        if (dueDate.Date < borrowDate.Date)
            throw new InvalidOperationException("Due date cannot be before the borrow date.");
    }
}
