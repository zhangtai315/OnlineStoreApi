using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace CMCOnlineStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ILogger<ImageController> logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ImageController(ILogger<ImageController> logger, IWebHostEnvironment webHostEnvironment)
        {
            this.logger = logger;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("small/{name}")]
        public async Task<IActionResult> Get(string name)
        {
            var path = Path.Combine(webHostEnvironment.ContentRootPath, "images\\small\\" + name);
            var image = System.IO.File.OpenRead(path);
            return File(image, "image/jpeg");
        }
    }
}
