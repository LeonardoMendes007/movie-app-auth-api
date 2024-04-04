using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Security.Claims;

namespace MovieApp.AuthApi.Identity.Model;
public class Credential
{
    public string id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public List<Claim> Claims { get; set; }
}
