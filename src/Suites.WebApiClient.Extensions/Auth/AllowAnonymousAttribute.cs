using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiClient.Attributes;
using WebApiClient.Contexts;

namespace WebApiClient.Extensions.Auth
{
    public class AllowAnonymousAttribute : ApiActionAttribute
    {
        public override Task BeforeRequestAsync(ApiActionContext context)
            => Task.CompletedTask;
    }
}
