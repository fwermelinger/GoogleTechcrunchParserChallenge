using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParserChallenge.Core
{
    public class TechCrunchParser : BaseParser
    {
        public TechCrunchParser(string html)
            : base(html)
        {

        }

        public List<Entities.TechCrunchResult> GetResults()
        {

            var nodes = this.htmlDocument.DocumentNode.SelectNodes(".//div[@class='block-content']");

            if (nodes != null)
            {               

                var query = from n in nodes
                            where n.SelectSingleNode(".//p[@class='excerpt']") != null
                            select new Entities.TechCrunchResult()
                            {
                                Title = System.Web.HttpUtility.HtmlDecode(n.SelectSingleNode(".//h2[@class='post-title']").InnerText),
                                Url = System.Web.HttpUtility.HtmlDecode(n.SelectSingleNode(".//h2[@class='post-title']//a").Attributes["href"].Value),
                                Author = System.Web.HttpUtility.HtmlDecode(n.SelectSingleNode(".//div[@class='byline']//a").InnerText),
                                Excerpt = System.Web.HttpUtility.HtmlDecode(n.SelectSingleNode(".//p[@class='excerpt']").InnerText)

                            };
                return query.ToList();
            }


            return new List<Entities.TechCrunchResult>();
        }
    }
}
