using BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace CMCOnlineStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly ILogger<ShippingController> logger;
        private readonly IBL bl;
        public ShippingController(ILogger<ShippingController> logger, IBL bl)
        {
            this.logger = logger;
            this.bl = bl;
        }

        [HttpGet("shippingcost/{orderValue}")]
        public async Task<ActionResult<decimal>> Get(decimal orderValue)
        {
            return Ok(bl.CalculateShippingCost(orderValue));
        }
    }
}
