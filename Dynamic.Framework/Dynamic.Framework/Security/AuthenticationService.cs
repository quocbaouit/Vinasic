using System;

namespace Dynamic.Framework.Security
{
    public class AuthenticationService : IAuthenticationService
    {
        public void Login(string token)
        {
            Authentication.Login(Convert.ToInt32(token));
        }

        public void Logout()
        {
            Authentication.Logout();
        }
    }
}
