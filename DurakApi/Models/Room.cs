namespace DurakApi.Models
{
    public class Room
    {
        public Guid Id {  get; private set; }
        public List<User> Users { get; set; } = [];
        public bool IsPlaying { get; private set; } = false;

        public Room() 
        { 
            Id = Guid.NewGuid();
        }

        public bool CanPlay => Users.Count > 1;
    }
}
