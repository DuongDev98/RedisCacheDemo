using Microsoft.AspNetCore.Mvc;
using RedisCacheDemo.Attributes;
using RedisCacheDemo.Services;

namespace RedisCacheDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly ICacheService _cacheService;
        public CacheController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpGet("{from}/{to}")]
        [Cache]
        public IActionResult Get(int from, int to)
        {
            List<string> list = new List<string>();
            for (int i = from; i < to; i++) list.Add(i.ToString());
            return Ok(list);
        }

        [HttpGet("insert")]
        public IActionResult Update()
        {
            _cacheService.Remove("");
            return Ok();
        }
    }
}
