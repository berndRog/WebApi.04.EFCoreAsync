using System;
using System.Text.RegularExpressions;
namespace WebApi.Core.Misc; 
public static class Utils {
   public static string As8(this Guid guid) => guid.ToString()[..8];
   public static string AsIban(this string s) => Regex.Replace(s, ".{4}", "$0 ").Trim();
}