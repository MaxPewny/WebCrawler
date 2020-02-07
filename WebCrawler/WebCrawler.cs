using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;

namespace WebCrawler
{
    class WebCrawler
    {
        static HashSet<string> mReadLinks = new HashSet<string>();
        static object mutex = new object();

        public void FindExpressions(string pUrl, string pSearchedExpression, int pSearchDepth)
        {
            bool containsUrl;

            lock (mutex)
            {
                containsUrl = mReadLinks.Contains(pUrl);
            }

            if (pSearchDepth > 0 && !containsUrl)
            {
                int expressionsCount = 0;
                string content;
                List<Thread> threads = new List<Thread>();

                //Console.WriteLine("NEW WEBSITE: {0}", pUrl);
        
                lock (mutex)
                {
                    mReadLinks.Add(pUrl);
                }
                try
                {
                    using (var wc = new System.Net.WebClient())
                    {
                        content = wc.DownloadString(pUrl);
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
                    Thread thread = new System.Threading.Thread(() =>
                        {
                            FindExpressions(link, pSearchedExpression, pSearchDepth - 1);
                        });
                    threads.Add(thread);
                    thread.Start();
                }

                foreach (var thread in threads)
                {
                    thread.Join();
                }

                
            }
            
        }


        
    }
}
