using System;

namespace Jammo.HTTP
{
    public static class Extensions
    {
        internal static string UntilOrEmpty(this string text, string stopAt = "-")
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            
            var charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

            return charLocation > 0 ? text[..charLocation] : string.Empty;
        }
    }
}