using System;

namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            WebCrawler webCrawler = new WebCrawler();

            webCrawler.FindExpressions("http://www.games-academy.de/", "Game", 3);
        }
    }
}
