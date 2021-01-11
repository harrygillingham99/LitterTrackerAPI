using System;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace store_api.Helpers
{
    /*
    Helper class for HttpRequest to extract the firebase token from the header and verify it.
    */
    public static class FirebaseAuthHelper
    {
        public static async Task<string> AuthorizeWithFirebase(this HttpRequest request)
        {
            try
            {
                var authHeader = request.Headers["Authorization"].FirstOrDefault();

                if (authHeader == null || authHeader == "not-logged-in")
                    return null;

                var decodedToken = FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(authHeader);

                return (await decodedToken).Uid;
            }
            catch (Exception e)
            {
                Log.Information(e,$"Unauthorized user request token: {request.Headers["Authorization"].FirstOrDefault()}");
                return null;
            }
        }
    }
}
