namespace library_system
{
    public class BorrowDetail
    {
        public int BorrowDetailID { get; set; }

        public int BorrowID { get; set; }

        public int BookID { get; set; }

        public DateTime? ReturnDate { get; set; }

        public decimal Fine { get; set; }

        public string Status { get; set; } = "Borrowed";
    }
}