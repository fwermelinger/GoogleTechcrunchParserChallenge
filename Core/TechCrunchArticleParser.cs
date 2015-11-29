using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParserChallenge.Core
{
    public class TechCrunchArticleParser : BaseParser
    {
        public TechCrunchArticleParser(string html)
            : base(html)
        {

        }

        /// <summary>
        /// Returns a dictionary that contains each word that occurs in the article and counts number of occurences
        /// </summary>
        /// <returns></returns>
        internal Dictionary<string, int> GetWordList()
        {
            var result = new Dictionary<string, int>();

            var node = this.htmlDocument.DocumentNode.SelectSingleNode(".//div[@class='article-entry text']");

            if (node != null)
            {
                var paragraphs = node.SelectNodes(".//p");
                var allTexts = paragraphs.Select(x => x.InnerText);
                var completeContent = String.Join(" ", allTexts);

                //first split words and make sure they are all lowercase
                var allwords = completeContent.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Distinct();
                allwords = (from w in allwords
                           select w.ToLowerInvariant()).Distinct().ToList();

                foreach (string searchTerm in allwords)
                {
                    int occurences = (from w in allwords
                                      where w == searchTerm
                                      select w).Count();

                    result.Add(searchTerm.ToLowerInvariant(), occurences);
                }
            }
            return result;
        }
    }
}
