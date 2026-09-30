namespace library_system
{
    public class Member
    {
        public int MemberID { get; set; }

        public string Name { get; set; } = "";

        public string Gender { get; set; } = "";

        public string Phone { get; set; } = "";

        public string Email { get; set; } = "";

        public string Address { get; set; } = "";

        public DateTime RegisterDate { get; set; }

        public string Status { get; set; } = "Active";
    }
}