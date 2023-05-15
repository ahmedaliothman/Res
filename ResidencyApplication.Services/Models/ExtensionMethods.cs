using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResidencyApplication.Services.Models
{
    public static class ExtensionMethods
    {
        public static string CheckValidity(this string s)
        {
            return s;
        }

        public static bool isInt(this string s, out int result)
        {

            return int.TryParse(s, out result);
        }
    }
}