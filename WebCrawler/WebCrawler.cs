using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WebCrawler
{
    class WebCrawler
    {
        static HashSet<string> mReadLinks = new HashSet<string>(); // HashSet to store Links that were already opened and analysed

        // Recursivly search Websites for a certain Expression using Tasks, you can also define how many pages should be read
        public async Task FindExpressions(string pUrl, string pSearchedExpression, int pSearchDepth = 1) 
        {
            if (pSearchDepth > 0 && !mReadLinks.Contains(pUrl)) // Checks if Url has already been read or Serach Depth has been reached
            {
                int expressionsCount = 0; // Variable to Count how many times the expression has been found ont the page
                string content; // variable 
                List<Task> tasks = new List<Task>(); // List of Tasks

                //Console.WriteLine("NEW WEBSITE: {0}", pUrl);
        
                mReadLinks.Add(pUrl);

                try
                {
                    using (var wc = new System.Net.WebClient())
                    {
                        content = await wc.DownloadStringTaskAsync(pUrl); // Starts and waits for downloading the webpage
                    }
                }
                catch (System.Net.WebException e) 
                {
                    Console.WriteLine("EXCEPTION AT {0} : {1}", pUrl, e.Message); // Catches and prints any exception that occured downloading the page
                    return;
                }
   


                foreach (Match match in Regex.Matches(content, pSearchedExpression))
                {
                    ++expressionsCount;
                }
                Console.WriteLine("EXPRESSION '{0}' FOUND IN {1} : {2}", pSearchedExpression, pUrl, expressionsCount); // Prints how many times the expression has been found

                // for each link found create a new Task to search the associated page
                foreach (Match match in Regex.Matches(content, "href=\"(https?://www\\.games-academy\\.de)?/([^\"#\\?:.]*)[\"#\\?]"))
                {
                    var link = "http://www.games-academy.de/" + match.Groups[2].Value;
                    //Console.WriteLine("LINK: {0}", link);
                    Task task = FindExpressions(link, pSearchedExpression, pSearchDepth - 1);
                    tasks.Add(task);
                }
                await Task.WhenAll(tasks.ToArray()); // starts all tasks on new thread and waits for them to finish
            }
        }
    }
}
