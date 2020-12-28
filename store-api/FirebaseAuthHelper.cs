using System;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using Serilog;

namespace store_api
{
    public static class FirebaseAuthHelper
    {
        public static async Task<string> Verify(this string token)
        {
            try
            {
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance
                    .VerifyIdTokenAsync(token);
                return decodedToken.Uid;
            }
            catch (Exception e)
            {
                Log.Information(e,$"Unauthorised user request token: {token}");
                return null;
            }
        }
    }
}
