using System;

namespace SiteCrawler.Core
{
    public static class GuardClauses
    {
        public static void IsValidLink(string link)
        {
            if (link == null)
            {
                throw new ArgumentNullException(nameof(link), "Value can not be null.");
            }

            if (!Uri.IsWellFormedUriString(link, UriKind.Absolute))
            {
                throw new ArgumentException("Well formated url string expected.");
            }
        }
    }
}
