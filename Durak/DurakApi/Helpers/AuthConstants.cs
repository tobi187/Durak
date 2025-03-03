using System.Security.Claims;

namespace DurakApi.Helpers;

public class AuthHelper
{
    public const string LoggedIn = "LoggedInPolicy";

    public static bool IsLoggedIn(ClaimsPrincipal principal) => principal.FindFirstValue("SecurityStamp") == "AspNet.Identity.SecurityStamp";

    public static Guid? FindId(ClaimsPrincipal principal) =>  
        Guid.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;
}
