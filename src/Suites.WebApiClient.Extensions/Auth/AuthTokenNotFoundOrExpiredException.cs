using System;
using System.Collections.Generic;
using System.Text;
using static WebApiClient.Extensions.Properties.Resources;

namespace WebApiClient.Extensions.Auth
{
    public class AuthTokenNotFoundOrExpiredException : Exception
    {
        public AuthTokenNotFoundOrExpiredException()
            : base(AuthTokenNotFoundOrExpiredExceptionString)
        { }
    }
}
