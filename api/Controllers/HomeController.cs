using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStorage _storage;
        public HomeController(ILogger<HomeController> logger, IStorage storage)
        {
            _logger = logger;
            _storage = storage;
        }

        [HttpPost]
        public IActionResult Post(string value)
        {
            var id = Request.Headers["id"];
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            _storage.TryAdd(id, Convert.ToByte(value));
            return Ok();
        }

        [Route("reset")]
        [HttpPost]
        public IActionResult Reset()
        {
            _storage.Reset();
            return Ok();
        }

        [HttpGet]
        public IList<CardItem> Get(string value)
        {
            return _storage.GetAll();
        }
    }
}