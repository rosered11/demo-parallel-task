using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Parallel.Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsynchonizeController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        public AsynchonizeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet("{aliasUrl}/{port}/{param1}/{param2}")]
        public async Task<ActionResult> Get(string aliasUrl, int port, int param1, int param2)
        {
            var client = _clientFactory.CreateClient("client");
            var master = new List<Int32>();
            Task<Task<Int32>> task = System.Threading.Tasks.Task.Factory.StartNew(async () => {
                var response = await client.GetAsync($"http://localhost:{port}/api/{aliasUrl}/{param1}");
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return Convert.ToInt32(result);
            });
            
            Task<Task<Int32>> task2 = System.Threading.Tasks.Task.Factory.StartNew(async () => {
                var response = await client.GetAsync($"http://localhost:{port}/api/{aliasUrl}/{param2}");
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return Convert.ToInt32(result);
            });
            
            Int32 summary = 0;
            System.Threading.Tasks.Task allTask = await System.Threading.Tasks.Task.Factory.ContinueWhenAll(new [] { task, task2 },async tasks =>
            {

                foreach(var task in tasks){
                    
                    Task<Int32> result = await task;
                    summary += await result;
                }
            });
            await allTask;
            return Ok(summary);
        }
    }
}