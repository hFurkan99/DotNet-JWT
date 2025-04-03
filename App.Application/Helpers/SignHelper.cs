using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace App.Application.Helpers
{
    public static class SignHelper
    {
        public static SecurityKey GetSymmetricSecurityKey(string securityKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        }
    }
}
