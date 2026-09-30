namespace library_system
{
    public class Book
    {
        public int BookID { get; set; }

        public string ISBN { get; set; } = "";

        public string Title { get; set; } = "";

        public string Author { get; set; } = "";

        public int CategoryID { get; set; }

        public string Publisher { get; set; } = "";

        public int PublishYear { get; set; }

        public int Quantity { get; set; }

        public int AvailableQuantity { get; set; }
    }
}