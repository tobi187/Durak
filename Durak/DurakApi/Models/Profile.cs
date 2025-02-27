namespace DurakApi.Models;

public class Profile
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public bool IsTempUser { get; set; }

}
