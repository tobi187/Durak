namespace DurakApi.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string? RoomId { get; set; }
        public Room? Room { get; set; }
    }
}
