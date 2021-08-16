namespace UTask.Services.Cryptography
{
    public interface ICryptographyService
    {
        string GetPasswordSHA3Hash(string password);
    }
}