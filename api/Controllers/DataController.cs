using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly IStorage _cardItemStorage;

        public DataController(IStorage cardItemStorage)
        {
            _cardItemStorage = cardItemStorage;
        }

        [HttpPost("item")]
        public IActionResult TryAddItem(CardItem cardItem)
        {
            _cardItemStorage.TryAdd(cardItem.Id, cardItem.Value);
            return Ok(cardItem.Id);
        }

        [HttpGet("items")]
        public IActionResult GetAllItems()
        {
            var items = _cardItemStorage.GetAll();
            return Ok(items);
        }

        [HttpPost("reset")]
        public IActionResult ResetData()
        {
            _cardItemStorage.Reset();
            return Ok();
        }
    }
}
