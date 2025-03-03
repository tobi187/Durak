using DurakApi.Db;
using DurakApi.Helpers;
using System.Security.Claims;

namespace DurakApi.Models.Db;

public class Profile : DbBase
{
    public string Username { get; set; }
    public bool IsTempUser { get; set; }
    public string? RoomId { get; set; }
    public Room? Room { get; set; }

    public static Profile New(ClaimsPrincipal principal)
    {
        return new Profile
        {
            Id = AuthHelper.FindId(principal)!.Value,
            IsTempUser = !AuthHelper.IsLoggedIn(principal),
            Username = TempCringe.GetRandomName(),
            UpdatedAt = DateTime.Now,
            CreatedAt = DateTime.Now,
        };
    }
}
