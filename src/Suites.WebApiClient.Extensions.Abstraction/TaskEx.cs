using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiClient.Extensions
{
    internal static class TaskEx
    {
        public static readonly Task CompletedTask = Task.FromResult<object>(null);
    }
}
