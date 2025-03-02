using DurakApi.Db;

namespace DurakApi.Models;

public class Profile : DbBase
{
    public string Username { get; set; }
    public bool IsTempUser { get; set; }
}
