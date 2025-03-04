using DurakApi.Db;
using DurakApi.Models.Api;

namespace DurakApi.Models.Db;

public class Room : DbBase
{
    public string Name { get; set; }
    public bool IsPlaying { get; set; } = false;
    public bool IsPrivate { get; set; } = false;
    public List<Profile> Users { get; set; }
    public GameRule? Rules { get; set; }

    public bool CanPlay() => Users?.Count > 1;
    public Profile Creator() => Users[0];

    public bool IsJoinable()
    {
        var ct = Users?.Count ?? 1;
        var playerLimit = Rules?.PlayerLimit ?? 4;
        return ct < playerLimit && !IsPlaying && ct > 0;
    }

    public static Room New(Profile user, string? name)
    {
        return new Room
        {
            Name = name ?? Guid.NewGuid().ToString(),
            Users = [user],
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
        };
    }

    public RoomInfoModelS ToRoomInfo() => new(Id, Name, Users?.Count ?? 1);
}

public class GameRule : DbBase
{
    public int PlayerLimit { get; set; } = 4;
    public int MinCard { get; set; } = 6;
    public bool PushAllowed { get; set; } = true;
    public int MaxBoardCardAmount { get; set; } = 6;
}
