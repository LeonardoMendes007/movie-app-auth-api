namespace MovieApp.AuthApi.Identity.Model;
public class AuthResult
{
    public Credential? Credential { get; private set; } = null;
    public bool IsAuthenticated { get; private set; } = false;

    public AuthResult(Credential credential, bool isAuthenticated)
    {
        Credential = credential;
        IsAuthenticated = isAuthenticated;
    }

    public AuthResult() { }
}
