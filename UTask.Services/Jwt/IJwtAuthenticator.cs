using UTask.Models;

namespace UTask.Services.Jwt
{
    public interface IJwtAuthenticator
    {
        string Authenticate(UserCredentials userCredentials);
    }
}