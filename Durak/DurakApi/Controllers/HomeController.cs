using Serilog;
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

    [HttpPost("/anon")]
    public async Task<IResult> LoginAnonymous(bool useCookies = true)
    {
        try
        {
            Claim[] claims = [new (ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())];
            var idenScheme = useCookies ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;
            var iden = new ClaimsIdentity(claims, idenScheme);
            var principa = new ClaimsPrincipal(iden);
            var props = new AuthenticationProperties();
            props.IsPersistent = false;
            props.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1);
            await HttpContext.SignInAsync(idenScheme, principa, props);
            return TypedResults.Empty;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Ex at Anon Login");
            return TypedResults.BadRequest();
        }
    }
}
