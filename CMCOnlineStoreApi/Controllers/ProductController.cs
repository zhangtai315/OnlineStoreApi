using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CMCOnlineStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> logger;
        private readonly IProductRepository repository;

        public ProductController(ILogger<ProductController> logger, IProductRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get(string? orderBy = null, string? searchText = null)
        {
            //validate orderby before getting data
            if (orderBy != null)
            {
                List<string> validationResult = Helper.GetInvalidOrderingFields<ProductDto>(orderBy.Split(","));
                if (validationResult.Any())
                {
                    string badFields = string.Join(" ", validationResult.ToArray());
                    return BadRequest($"Cannot sort data by fields(s) {badFields}");
                }
            }

            IEnumerable<ProductDto> Products = (await repository.GetProductsAsync(searchText))
                        .Select(a => a.AsDto());

            if (orderBy != null)
            {
                Products = Products.OrderBy<ProductDto>(orderBy);
            }

            logger.LogInformation($"{DateTime.UtcNow:hh:mm:ss}: found {Products.Count()} Products");

            //return Ok(Products);
            return Ok(Products);
        }
    }
}