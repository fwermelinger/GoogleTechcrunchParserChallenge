using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParserChallenge.Core
{
    public class GoogleResultsParser : BaseParser
    {
        public GoogleResultsParser(string html) : base(html)
        {            

        }

        public List<Entities.GoogleResult> GetResults()
        {
            var nodes = this.htmlDocument.DocumentNode.SelectNodes("//li[@class='g']");         

            var query = from n in nodes
                        select new Entities.GoogleResult()
                        {
                            Title = System.Web.HttpUtility.HtmlDecode(n.SelectSingleNode(".//h3[@class='r']").InnerText),
                            Url = System.Web.HttpUtility.HtmlDecode(n.SelectSingleNode(".//h3[@class='r']//a").Attributes["href"].Value)
                        };

            return query.ToList();
        }

    }
}
