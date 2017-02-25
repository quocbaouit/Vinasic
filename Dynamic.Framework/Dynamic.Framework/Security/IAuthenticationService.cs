namespace Dynamic.Framework.Security
{
    public interface IAuthenticationService
    {
        void Login(string token);

        void Logout();
    }
}
