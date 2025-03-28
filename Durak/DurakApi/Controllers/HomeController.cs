using Serilog;
using DotNetEnv;
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
    public IActionResult Health() {
        if (Env.GetString("ASPNETCORE_ENVIRONMENT", "Production") == "Development")
            return Redirect("/swagger");
        return Ok(new { status = "(barely) alive" });
    }

    [HttpPost("/anon")]
    public async Task<IResult> LoginAnonymous(bool useCookies = true) {
        try {
            Claim[] claims = [new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())];
            var idenScheme = useCookies ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;
            var iden = new ClaimsIdentity(claims, idenScheme);
            var principal = new ClaimsPrincipal(iden);
            var props = new AuthenticationProperties();
            props.IsPersistent = !false; // session cookie i sink <- maybe change this on prod ?
            props.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1);
            await HttpContext.SignInAsync(idenScheme, principal, props);
            return TypedResults.Empty;
        } catch (Exception ex) {
            Log.Error(ex, "Ex at Anon Login");
            return TypedResults.BadRequest();
        }
    }
}
