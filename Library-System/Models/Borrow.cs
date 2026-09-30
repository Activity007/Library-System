namespace library_system
{
    public class Borrow
    {
        public int BorrowID { get; set; }

        public int MemberID { get; set; }

        public int LibrarianID { get; set; }

        public DateTime BorrowDate { get; set; }

        public DateTime DueDate { get; set; }

        public string Status { get; set; } = "Borrowed";
    }
}