using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestYaml.Extensions
{
    public static class DictionaryExtensions
    {
        public static IEnumerable<string> ToCookie(this Dictionary<string, string> cookie)
        {
            return cookie.Select(c => $"{c.Key}={c.Value};");
        }
    }
}
