using MovieApp.AuthApi.Identity.Model;

namespace MovieApp.AuthApi.Identity.Interfaces.Services;
public interface IAuthService
{
    public Task<AuthResult> RegisterAccount(string userName, string email, string password);
    public Task<AuthResult> SignInAsync(string email, string password);

}
