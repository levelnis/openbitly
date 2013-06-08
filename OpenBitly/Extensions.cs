using System;

namespace OpenBitly
{
    public static class Extensions
    {
        public static DateTime AsDateTime(this int timestamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return dateTime.AddSeconds(timestamp);
        } 

        public static DateTime AsLocalDateTime(this int timestamp)
        {
            return timestamp.AsDateTime().ToLocalTime();
        } 
    }
}