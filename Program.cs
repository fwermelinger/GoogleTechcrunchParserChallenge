using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParserChallenge
{
    class Program
    {
        private static bool USE_TESTDATA = true;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Write("Argument 1 (search query) required");
                return;
            }

            DoGoogleTask(args.First());          
            
            DoTechCrunchTask();          

            //just to prevent console from closing
            Console.ReadKey();
        }

        /// <summary>
        /// Loads the TechCrunch frontpage and displays all results
        /// </summary>
        private static void DoTechCrunchTask()
        {
            Console.WriteLine("Getting TechCrunch Results:");
            Console.WriteLine("===========================");

            string resultHtml = "";

            if (USE_TESTDATA)
            {
                resultHtml = System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Testdata\\techcrunchresult.txt");
            }
            else
            {
                var uri = "http://techcrunch.com/";
                var resultPage = new Core.WebPageDownloader(uri);

                resultPage.DownloadPageAsync().Wait();
                resultHtml = resultPage.Result;
            }

            var parser = new Core.TechCrunchParser(resultHtml);
            var resultList = parser.GetResults();

            foreach (var res in resultList)
            {
                Console.WriteLine("{0} (by {1})\n{2}\n{3}\n", res.Title, res.Author, res.Excerpt, res.Url);
            }

            Console.WriteLine("=== End of TechCrungh Results ===" + Environment.NewLine);
        }

        /// <summary>
        /// Loads the Google Results for a certain term and displays all results
        /// </summary>
        /// <param name="searchterm"></param>
        private static void DoGoogleTask(string searchterm)
        {
            Console.WriteLine("Getting Google Results:");
            Console.WriteLine("=======================");

            string resultHtml = "";

            if (USE_TESTDATA)
            {
                resultHtml = System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Testdata\\googleresult.txt");
            }
            else
            {                
                var uri = string.Format("https://www.google.com/search?q={0}", System.Web.HttpUtility.UrlEncode(searchterm));
                var searchResultPage = new Core.WebPageDownloader(uri);

                searchResultPage.DownloadPageAsync().Wait();
                resultHtml = searchResultPage.Result;
            }

            var googleParser = new Core.GoogleResultsParser(resultHtml);
            var googleResults = googleParser.GetResults();

            foreach (var res in googleResults.OrderBy(x => x.Title))
            {
                Console.WriteLine("{0}\n{1}\n", res.Title, res.Url);
            }

            Console.WriteLine("=== End of Google Results ===" + Environment.NewLine);
        }

        
    }
}