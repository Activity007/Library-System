using library_system;
using Npgsql;
using System.Reflection;

internal static class Program
{
    private static int checks;

    [STAThread]
    private static int Main()
    {
        string? originalConnection = Environment.GetEnvironmentVariable("LIBRARY_DB_CONNECTION");
        using var configured = new Database().GetConnection();
        var builder = new NpgsqlConnectionStringBuilder(configured.ConnectionString) { Database = "postgres" };
        string databaseName = "library_system_test_" + Guid.NewGuid().ToString("N");
        using var admin = new NpgsqlConnection(builder.ConnectionString);
        bool created = false;
        try
        {
            admin.Open();
            using (var create = new NpgsqlCommand($"CREATE DATABASE {databaseName}", admin)) create.ExecuteNonQuery();
            created = true;
            builder.Database = databaseName;
            Environment.SetEnvironmentVariable("LIBRARY_DB_CONNECTION", builder.ConnectionString);
            Execute(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "schema.sql")));
            RunChecks();
            Console.WriteLine($"PASS: {checks} checks, using an isolated PostgreSQL database.");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            return 1;
        }
        finally
        {
            Environment.SetEnvironmentVariable("LIBRARY_DB_CONNECTION", originalConnection);
            NpgsqlConnection.ClearAllPools();
            if (created)
            {
                // Only the uniquely named database created by this test run is removed.
                using var drop = new NpgsqlCommand($"DROP DATABASE {databaseName} WITH (FORCE)", admin);
                drop.ExecuteNonQuery();
            }
        }
    }

    private static void RunChecks()
    {
        Execute("INSERT INTO Librarian (Name, Username, Password) VALUES ('Test Librarian', 'tester', @hash)",
            ("hash", LoginService.HashPassword("test-password")));
        var login = new LoginService();
        var librarian = login.Authenticate("tester", "test-password")!;
        Check(librarian != null && librarian.LibrarianID > 0, "Database-backed login");
        Check(login.Authenticate("tester", "wrong") == null, "Wrong password rejected");
        Check(login.Authenticate("missing", "test-password") == null, "Unknown user rejected");
        Check(!LoginService.VerifyPassword("1234", "1234"), "Plaintext password rejected");

        var categories = new CategoryService();
        categories.AddCategory(new Category { CategoryName = "Discard" });
        categories.DeleteCategory(categories.GetAllCategories().Single().CategoryID);
        categories.AddCategory(new Category { CategoryName = "Computing", Description = "Technical books" });
        var category = categories.GetAllCategories().Single();
        Check(category.CategoryID > 1, "Database IDs are not list positions");
        var members = new MemberService();
        members.AddMember(new Member { Name = "Reader One", RegisterDate = DateTime.Today });
        members.AddMember(new Member { Name = "Reader Two", RegisterDate = DateTime.Today });
        var member = members.SearchMembers("One").Single();
        var secondMember = members.SearchMembers("Two").Single();
        member.Phone = "012345678";
        members.UpdateMember(member);
        Check(new MemberService().SearchMembers("012345678").Single().MemberID == member.MemberID,
            "Member edits persist across service instances");

        var books = new BookService();
        books.AddBook(new Book { ISBN = "TEST-A", Title = "C#", Author = "Author A", CategoryID = category.CategoryID,
            PublishYear = 2025, Quantity = 2, AvailableQuantity = 999 });
        books.AddBook(new Book { ISBN = "TEST-B", Title = "Databases", Author = "Author B", CategoryID = category.CategoryID,
            PublishYear = 2024, Quantity = 1 });
        var first = books.SearchBooks("TEST-A").Single();
        var second = books.SearchBooks("TEST-B").Single();
        Check(first.AvailableQuantity == 2, "Initial availability follows total quantity");
        category.CategoryName = "Computer Science";
        categories.UpdateCategory(category);
        Check(categories.SearchCategories("Science").Single().CategoryID == category.CategoryID,
            "Category rename preserves its ID");
        ExpectFailure(() => categories.DeleteCategory(category.CategoryID), "Referenced category cannot be deleted");

        using (var form = new BookForm())
        {
            Invoke(form, "LoadBooks");
            var grid = Field<DataGridView>(form, "dgvBooks");
            Check(grid.Rows.Count == 2 && (string)grid.Rows[0].Cells["Category"].Value! == "Computer Science",
                "Books screen joins saved category names");
            var combo = Field<ComboBox>(form, "cmbCategory");
            Check(combo.Items.Count == 1 && combo.Items[0] is Category, "Category choices contain live records only");
        }
        using (var form = new MemberForm())
        {
            Invoke(form, "LoadMembers");
            Check(Field<DataGridView>(form, "dgvMembers").Rows.Count == 2, "Member screen reloads saved records");
        }
        using (var form = new CategoriesForm())
        {
            Invoke(form, "LoadCategories");
            Check(Field<DataGridView>(form, "dgvCategories").Rows.Count == 1, "Categories screen reloads saved records");
        }

        var borrow = new BorrowService();
        var history = new HistoryService();
        DateTime borrowDate = DateTime.Today.AddDays(-10), dueDate = DateTime.Today.AddDays(-3);
        ExpectFailure(() => borrow.BorrowBooks(member.MemberID, librarian!.LibrarianID, borrowDate, dueDate, []), "Empty loan rejected");
        ExpectFailure(() => borrow.BorrowBooks(member.MemberID, librarian!.LibrarianID, borrowDate, dueDate, [first.BookID, first.BookID]), "Duplicate books rejected");
        ExpectFailure(() => borrow.BorrowBooks(member.MemberID, librarian!.LibrarianID, DateTime.Today, dueDate, [first.BookID]), "Invalid due date rejected");
        int borrowID = borrow.BorrowBooks(member.MemberID, librarian!.LibrarianID, borrowDate, dueDate, [first.BookID, second.BookID]);
        Check(books.SearchBooks("TEST-A").Single().AvailableQuantity == 1 && books.SearchBooks("TEST-B").Single().AvailableQuantity == 0,
            "Borrow decrements all selected books");
        var loans = history.GetLoans(borrowID: borrowID);
        Check(loans.Count == 2 && loans.All(loan => loan.MemberName == member.Name && loan.LibrarianName == librarian.Name),
            "History links member, books, and signed-in librarian");
        Check(history.GetLoans(status: "Overdue").Count == 2, "Overdue status comes from dates");
        Check(history.GetLoans(from: DateTime.Today, to: DateTime.Today).Count == 0, "History date filter works");
        Check(history.GetLoans("Databases").Count == 1, "History text filter works");
        Check(history.GetDashboard()["Borrow"] == "2 copies on loan", "Dashboard reflects borrowing");

        using (var form = new MainMenuForm(librarian))
        {
            Invoke(form, "RefreshDashboard");
            Check(Field<Label>(form, "lblWelcome").Text.Contains(librarian.Name), "Dashboard shows signed-in librarian");
            Check(Field<Dictionary<string, Label>>(form, "dashboardLabels")["Borrow"].Text == "2 copies on loan",
                "Dashboard screen displays live service totals");
        }

        using (var form = new BorrowForm(librarian))
        {
            Invoke(form, "LoadChoices");
            Check(Field<ComboBox>(form, "cmbBook").Items.Count == 1, "Unavailable books excluded from choices");
            Check(Field<ComboBox>(form, "cmbMember").Items.Count == 2, "Members populated from database");
        }
        using (var form = new ReturnForm())
        {
            Field<TextBox>(form, "txtBorrowID").Text = borrowID.ToString();
            Invoke(form, "LoadBorrow");
            Check(Field<DataGridView>(form, "dgvBorrowedBooks").Rows.Count == 2, "Return screen loads outstanding details");
            Check(Field<Label>(form, "lblFine").Text == "$3.00", "Return screen previews the saved due-date fine");
            Field<TextBox>(form, "txtBorrowID").Text = "99999";
            Check(Field<DataGridView>(form, "dgvBorrowedBooks").Rows.Count == 0, "Changing Borrow ID clears stale selection");
        }
        using (var form = new HistoryForm())
        {
            Invoke(form, "LoadHistory");
            Check(Field<DataGridView>(form, "dgvHistory").Rows.Count == 2, "History screen shows saved loans only");
        }

        int transactionCount = Scalar("SELECT COUNT(*) FROM Borrow");
        ExpectFailure(() => borrow.BorrowBooks(member.MemberID, librarian.LibrarianID, borrowDate, dueDate, [first.BookID, second.BookID]),
            "An unavailable book rejects the whole transaction");
        Check(Scalar("SELECT COUNT(*) FROM Borrow") == transactionCount && books.SearchBooks("TEST-A").Single().AvailableQuantity == 1,
            "Failed borrowing rolls back stock and header");
        first.Quantity = 4;
        first.AvailableQuantity = 99;
        books.UpdateBook(first);
        Check(books.SearchBooks("TEST-A").Single().AvailableQuantity == 3, "Stock edits preserve copies on loan");
        first.Quantity = 0;
        ExpectFailure(() => books.UpdateBook(first), "Cannot reduce total below loaned copies");
        Check(books.SearchBooks("TEST-A").Single().Quantity == 4, "Invalid stock edit leaves inventory unchanged");

        var returns = new ReturnService();
        var firstLoan = loans.Single(loan => loan.BookID == first.BookID);
        var secondLoan = loans.Single(loan => loan.BookID == second.BookID);
        ExpectFailure(() => returns.ReturnBook(firstLoan.BorrowDetailID, borrowDate.AddDays(-1)), "Return before borrowing rejected");
        Check(returns.ReturnBook(firstLoan.BorrowDetailID, DateTime.Today) == 3m, "Fine calculated in service");
        Check(Scalar($"SELECT COUNT(*) FROM Borrow WHERE BorrowID = {borrowID} AND Status = 'Borrowed'") == 1,
            "Partial return keeps transaction open");
        ExpectFailure(() => returns.ReturnBook(firstLoan.BorrowDetailID, DateTime.Today), "Duplicate return rejected");
        Check(books.SearchBooks("TEST-A").Single().AvailableQuantity == 4, "Duplicate return does not inflate stock");
        members.DeactivateMember(member.MemberID);
        Check(borrow.GetActiveMembers().Count == 1, "Inactive members excluded from borrowing");
        ExpectFailure(() => borrow.BorrowBooks(member.MemberID, librarian.LibrarianID, borrowDate, dueDate, [first.BookID]),
            "Inactive member rejected by service");
        returns.ReturnBook(secondLoan.BorrowDetailID, DateTime.Today);
        Check(Scalar($"SELECT COUNT(*) FROM Borrow WHERE BorrowID = {borrowID} AND Status = 'Returned'") == 1,
            "Final return closes transaction even for inactive member");
        Check(history.GetLoans(status: "Returned").Count == 2, "Returned history remains available");
        Check(history.GetDashboard()["Borrow"] == "0 copies on loan", "Dashboard reflects all returns");
        ExpectFailure(() => books.DeleteBook(first.BookID), "Books referenced by history cannot be deleted");

        // Two clients compete for the last copy.
        var attempts = Enumerable.Range(0, 2).Select(_ => Task.Run(() =>
        {
            try { return borrow.BorrowBooks(secondMember.MemberID, librarian.LibrarianID, DateTime.Today, DateTime.Today.AddDays(7), [second.BookID]); }
            catch (InvalidOperationException) { return 0; }
        })).ToArray();
        Task.WaitAll(attempts);
        Check(attempts.Count(task => task.Result > 0) == 1, "Concurrent borrows cannot oversell the last copy");
        int concurrentID = attempts.Single(task => task.Result > 0).Result;
        int detailID = history.GetLoans(borrowID: concurrentID).Single().BorrowDetailID;
        var returnAttempts = Enumerable.Range(0, 2).Select(_ => Task.Run(() =>
        {
            try { returns.ReturnBook(detailID, DateTime.Today); return true; }
            catch (InvalidOperationException) { return false; }
        })).ToArray();
        Task.WaitAll(returnAttempts);
        Check(returnAttempts.Count(task => task.Result) == 1 && books.SearchBooks("TEST-B").Single().AvailableQuantity == 1,
            "Concurrent duplicate returns restore exactly one copy");
        Check(LoanRules.CalculateFine(DateTime.Today, DateTime.Today.AddDays(-1)) == 0m, "Early returns have no fine");
        Check(Scalar("SELECT COUNT(*) FROM Book k WHERE k.Quantity - k.AvailableQuantity <> (SELECT COUNT(*) FROM BorrowDetail d WHERE d.BookID = k.BookID AND d.ReturnDate IS NULL)") == 0,
            "Inventory equals total copies minus outstanding details");
    }

    private static void Check(bool condition, string description)
    {
        if (!condition) throw new Exception("FAIL: " + description);
        checks++;
        Console.WriteLine("PASS: " + description);
    }

    private static void ExpectFailure(Action action, string description)
    {
        try { action(); }
        catch (Exception ex) when (ex is InvalidOperationException or PostgresException)
        {
            Check(true, description);
            return;
        }
        throw new Exception("FAIL: " + description);
    }

    private static T Field<T>(object target, string name) =>
        (T)target.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(target)!;

    private static void Invoke(object target, string name) =>
        target.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(target, null);

    private static void Execute(string sql, params (string Name, object Value)[] parameters)
    {
        using var connection = new Database().GetConnection();
        connection.Open();
        using var command = new NpgsqlCommand(sql, connection);
        foreach (var parameter in parameters) command.Parameters.AddWithValue(parameter.Name, parameter.Value);
        command.ExecuteNonQuery();
    }

    private static int Scalar(string sql)
    {
        using var connection = new Database().GetConnection();
        connection.Open();
        using var command = new NpgsqlCommand(sql, connection);
        return Convert.ToInt32(command.ExecuteScalar());
    }
}
