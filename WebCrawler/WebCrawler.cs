using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WebCrawler
{
    class WebCrawler
    {
        HashSet<string> mReadLinks = new HashSet<string>();

        public void FindExpressions(string pUrl, string pSearchedExpression, int pSearchDepth)
        {
            int expressionsCount = 0;
            string content;
            if (pSearchDepth > 0 && !mReadLinks.Contains(pUrl))
            {
                Console.WriteLine("NEW WEBSITE: {0}", pUrl);
                mReadLinks.Add(pUrl);
                using (var wc = new System.Net.WebClient())
                {
                    content = wc.DownloadString(pUrl);
                }

                foreach (Match match in Regex.Matches(content, pSearchedExpression))
                {
                    ++expressionsCount;
                }
                Console.WriteLine("EXPRESSION '{0}' FOUND: {1}", pSearchedExpression, expressionsCount);

                foreach (Match match in Regex.Matches(content, "href=\"(https?://www\\.games-academy\\.de)?/([^\"#\\?:.]*)[\"#\\?]"))
                {
                    var link = "http://www.games-academy.de/" + match.Groups[2].Value;
                    //Console.WriteLine("LINK: {0}", link);
                    FindExpressions(link, pSearchedExpression, pSearchDepth - 1);
                }

                
            }
            
        }


        
    }
}
