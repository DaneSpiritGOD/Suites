using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace System.Net
{
    public static class NetExtensions
    {
        public static bool IsValidPort(this int port)
        {
            return (port > IPEndPoint.MinPort && port < IPEndPoint.MaxPort);
        }

        public static bool IsValidIP(this string ip)
        {
            return (ip == "*" || IPAddress.TryParse(ip, out IPAddress ipAddr));
        }
    }
}
