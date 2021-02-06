using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Parallel.Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SynchonizeController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        public SynchonizeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet("{aliasUrl}/{port}/{param1}/{param2}")]
        public async Task<ActionResult> Get(string aliasUrl, int port, int param1, int param2)
        {
            var client = _clientFactory.CreateClient("client");
            var response = await client.GetAsync($"http://localhost:{port}/api/{aliasUrl}/{param1}");
            response.EnsureSuccessStatusCode();
            var result1 = await response.Content.ReadAsStringAsync();
            response = await client.GetAsync($"http://localhost:{port}/api/{aliasUrl}/{param2}");
            response.EnsureSuccessStatusCode();
            var result2 = await response.Content.ReadAsStringAsync();
            var expectResult = Convert.ToInt32(result1) + Convert.ToInt32(result2);
            return Ok(expectResult);
        }
    }
}