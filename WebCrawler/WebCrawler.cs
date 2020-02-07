using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WebCrawler
{
    class WebCrawler
    {
        static HashSet<string> mReadLinks = new HashSet<string>();

        public async Task FindExpressions(string pUrl, string pSearchedExpression, int pSearchDepth)
        {
            if (pSearchDepth > 0 && !mReadLinks.Contains(pUrl))
            {
                int expressionsCount = 0;
                string content;
                List<Task> tasks = new List<Task>();

                //Console.WriteLine("NEW WEBSITE: {0}", pUrl);
        
                mReadLinks.Add(pUrl);

                try
                {
                    using (var wc = new System.Net.WebClient())
                    {
                        content = await wc.DownloadStringTaskAsync(pUrl);
                    }
                }
                catch (System.Net.WebException e) 
                {
                    Console.WriteLine("EXCEPTION AT {0} : {1}", pUrl, e.Message);
                    return;
                }
   


                foreach (Match match in Regex.Matches(content, pSearchedExpression))
                {
                    ++expressionsCount;
                }
                Console.WriteLine("EXPRESSION '{0}' FOUND IN {1} : {2}", pSearchedExpression, pUrl, expressionsCount);

                foreach (Match match in Regex.Matches(content, "href=\"(https?://www\\.games-academy\\.de)?/([^\"#\\?:.]*)[\"#\\?]"))
                {
                    var link = "http://www.games-academy.de/" + match.Groups[2].Value;
                    //Console.WriteLine("LINK: {0}", link);
                    Task task = FindExpressions(link, pSearchedExpression, pSearchDepth - 1);
                    tasks.Add(task);
                }
                await Task.WhenAll(tasks.ToArray());
            }
            
        }


        
    }
}
