using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

            var articles = DoTechCrunchTask();

            DownloadAndAnalyzeTechCrunchArticles(articles);

            //just to prevent console from closing
            Console.ReadKey();
        }

        private static void DownloadAndAnalyzeTechCrunchArticles(List<Entities.TechCrunchResult> articles)
        {            
            var downloaders = (from a in articles
                              select new Core.WebPageDownloader(a.Url)).ToList();
            //gotcha: we have to call .tolist here so it creates the actual instance. Otherwise, it will create a clone at the Parallel.Foreach and also at the foreach loop below!

            var parallelResult = Parallel.ForEach(downloaders, new ParallelOptions() { MaxDegreeOfParallelism = 3 }, x => x.DownloadPageAsync().Wait());

            var globalWordCount = new Dictionary<string, int>();

            //loop through all downloaded pages
            foreach (var page in downloaders)
            {
                var articleParser = new Core.TechCrunchArticleParser(page.Result);
                var list = articleParser.GetWordList();

                //add words or their number of occurence to the global dictionary
                foreach (var word in list)
                {
                    if (globalWordCount.ContainsKey(word.Key))
                    {
                        globalWordCount[word.Key] += word.Value;
                    }
                    else globalWordCount.Add(word.Key, word.Value);
                }
            }

            Console.WriteLine("Word Count from result pages:");
            Console.WriteLine("=============================");

            foreach (var word in globalWordCount.OrderBy(x=> x.Value))
            {
                

                Console.WriteLine("{0} : {1}", word.Value.ToString().PadLeft(5, ' '), word.Key);
            }
        }

        /// <summary>
        /// Loads the TechCrunch frontpage and displays all results
        /// </summary>
        private static List<Entities.TechCrunchResult> DoTechCrunchTask()
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
            return resultList;
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