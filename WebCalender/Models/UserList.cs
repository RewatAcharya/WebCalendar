namespace WebCalender.Models
{
    public class UserList
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserPassword { get; set; } = null!;
        public string UserRole { get; set; } = null!;

    }
}
