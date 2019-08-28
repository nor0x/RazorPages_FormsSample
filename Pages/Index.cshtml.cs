using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorFormSample.Models;

namespace RazorFormSample.Pages
{
    public class IndexModel : PageModel
    {
        readonly IHttpClientFactory _clientFactory;
        readonly string API = "https://jsonplaceholder.typicode.com/posts";

        [BindProperty]
        public List<SamplePost> Posts { get; set; }

        [BindProperty]
        public string Query { get; set; }

        public IndexModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public void OnGet()
        {
            Query = "Hello World!";
            Posts = new List<SamplePost>();
        }

        public async Task OnPost()
        {
            using (var client = _clientFactory.CreateClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get,API);

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var allPosts = SamplePost.FromJson(json);
                    if (string.IsNullOrEmpty(Query))
                    {
                        Posts = allPosts;
                    }
                    else
                    {
                        Posts = allPosts.Where(p => p.Body.ToLower().Contains(Query.ToLower())).ToList();
                    }
                }
                else
                {
                    //Error handling
                    System.Diagnostics.Debug.WriteLine(response.StatusCode + " " + response.ReasonPhrase);
                }
            }

        }
    }
}
