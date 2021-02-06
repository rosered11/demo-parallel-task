using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Demo.Parallel.Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MathController : ControllerBase
    {
        [HttpGet("{number}")]
        public async Task<ActionResult> Get(int number){
            var service = new FactorialService();
            var result = await service.Sleep(number);
            return Ok(number);
        }
    }
}