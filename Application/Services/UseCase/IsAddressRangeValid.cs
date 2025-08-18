using System;
using System.Linq;
using System.Net;
using DataAccess.Models;

namespace Application.Services.UseCase
{
    public static class IsAddressRangeValid
    {
        public static bool Test(Referer referer, string address)
        {
            if (referer == null)
                throw new NullReferenceException(Message.null_Referer);

            if ((referer.ipStart == null) || (referer.ipEnd == null))
                throw new NullReferenceException(Message.null_ip);

            return IsAddressInRange(referer.ipStart, referer.ipEnd, address);
        }
        
        private static bool IsAddressInRange(string startIpAddr, string endIpAddr, string address)
        {
            long ipStart = BitConverter.ToInt32(IPAddress.Parse(startIpAddr).GetAddressBytes().Reverse().ToArray(), 0);
            long ipEnd = BitConverter.ToInt32(IPAddress.Parse(endIpAddr).GetAddressBytes().Reverse().ToArray(), 0);
            long ip = BitConverter.ToInt32(IPAddress.Parse(address).GetAddressBytes().Reverse().ToArray(), 0);
            return ip >= ipStart && ip <= ipEnd;
        }
    }
}