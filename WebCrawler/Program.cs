using System;

namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args) // searching the homepage to test functionality
        {
            WebCrawler webCrawler = new WebCrawler();
            webCrawler.FindExpressions("http://www.games-academy.de/", "Game", 10).Wait();
        }
    }
}
