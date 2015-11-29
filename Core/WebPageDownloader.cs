﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebParserChallenge.Core
{
    public class WebPageDownloader
    {
        public string Result { get; set; }
        public Uri Uri { get; set; }


        public WebPageDownloader(string uri)
        {
            this.Uri = new Uri(uri);
        }

        public async Task DownloadPageAsync()
        {            
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                //client.BaseAddress = this.Uri;
                var response = await client.GetAsync(Uri);
                this.Result = await response.Content.ReadAsStringAsync();

                
            }                       
        }
    }
}
