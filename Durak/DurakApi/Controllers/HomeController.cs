using Serilog;
using DurakApi.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;

namespace DurakApi.Controllers;

[Route("/")]
[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet("/")]
    public IActionResult Health()
    {
        return Ok(new { status = "alive" });
    }

    public record AnonUser(string? Name);

    [HttpPost("/anon")]
    public async Task<IResult> LoginAnonymous(AnonUser user)
    {
        try
        {
            Claim[] claims = [
                new (ClaimTypes.Name, user.Name ?? TempCringe.GetRandomName()),
                new (ClaimTypes.Anonymous, "true")];
            var iden = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
            var principa = new ClaimsPrincipal(iden);
            var props = new AuthenticationProperties();
            props.IsPersistent = false;
            props.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1);
            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principa, props);
            return TypedResults.Empty;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Ex at Anon Login");
            return TypedResults.BadRequest();
        }
    }
}
