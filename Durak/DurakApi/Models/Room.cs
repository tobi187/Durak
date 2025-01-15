namespace DurakApi.Models
{
    public class Room
    {
        public Guid Id {  get; private set; } = Guid.NewGuid();
        public string Name { get; set; }
        public List<User> Users { get; set; } = [];
        public bool IsPlaying { get; set; } = false;

        public bool CanPlay => Users.Count > 1;
    }
}
