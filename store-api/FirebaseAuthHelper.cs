using System;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using Serilog;
using store_api.Objects;
using store_api.Objects.StoreObjects;

namespace store_api
{
    public static class FirebaseAuthHelper
    {
        public static async Task<string> Verify(this AuthedRequest token)
        {
            try
            {
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance
                    .VerifyIdTokenAsync(token.JwtToken);
                return decodedToken.Uid;
            }
            catch (Exception e)
            {
                Log.Information(e,$"Unauthorised user request token: {token.JwtToken}");
                return null;
            }
        }
    }
}
