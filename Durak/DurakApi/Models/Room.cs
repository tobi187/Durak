using DurakApi.Db;

namespace DurakApi.Models;

public class Room : DbBase
{
    public string Name { get; set; }
    public bool IsPlaying { get; set; } = false;
    public Profile Creator { get; set; } 
    public List<Profile> Users { get; set; }
    public GameRule? Rules { get; set; }

    public bool CanPlay => Users.Count > 1;
}

public class GameRule : DbBase
{
    public int PlayerLimit { get; set; } = 4;
    public int MinCard { get; set; } = 6;
    public bool PushAllowed { get; set; } = true;
    public int MaxBoardCardAmount { get; set; } = 6;
}
