using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParserChallenge.Core
{
    public class BaseParser
    {
        private string RawHtml;
        public HtmlAgilityPack.HtmlDocument htmlDocument { get; set; }

        public BaseParser(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                throw new ArgumentException("html can not be empty");
            }

            this.RawHtml = html;

            this.htmlDocument = new HtmlAgilityPack.HtmlDocument();
            this.htmlDocument.LoadHtml(this.RawHtml);

        }
    }
}
