using System;

namespace Book.Common
{
    public class WebHelper
    {
        public static string Combine(Uri uri, string absUrl)
        {
            var absUri = new Uri(uri,absUrl);
            return absUri.ToString();
        }
    }
}
