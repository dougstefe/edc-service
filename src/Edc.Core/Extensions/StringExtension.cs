using System.Text;

namespace Edc.Core.Extensions;

public static class StringExtension {
    public static string ToBase64(this string text) => Convert.ToBase64String(Encoding.Default.GetBytes(text));

}
