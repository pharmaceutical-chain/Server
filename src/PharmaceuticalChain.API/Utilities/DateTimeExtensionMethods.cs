using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Utilities
{
    public static class DateTimeExtensionMethods
    {
        public static int ToUnixTimestamp(this DateTime dateTime)
        {
            int unixTimestamp = (int)dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            return unixTimestamp;
        }
    }
}
