using ImageProcessing.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImageProcessing.API.Controllers;

[ApiController]
[Route("[controller]")]
    public class ImagesController: ControllerBase
    {
        private readonly IBlobsManagement _blobsManagement;
        private readonly IQueuesManagement _queuesManagement;
        private readonly IConfiguration _configuration;
        public ImagesController(
            IBlobsManagement blobsManagement,
            IQueuesManagement queuesManagement,
            IConfiguration configuration
        )
        {
            _blobsManagement = blobsManagement;
            _queuesManagement = queuesManagement;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("ImageUpload")]
        public async Task<IActionResult> ImageUpload(IFormFile? img)
        {
            if(img == null)
                return BadRequest();

            await UploadFile(img, 300, 300);

            return Ok();
        }

        [NonAction]
        private async Task UploadFile(IFormFile img, int width, int height)
        {
            if(img is not {Length: >0}) return;

            var connection = _configuration["StorageConfig:BlobConnection"];

            byte[]? fileBytes = null;
            MemoryStream? stream = null;
            await using(stream = new MemoryStream())
            {
                await img.CopyToAsync(stream);
                fileBytes = stream.ToArray();
            }

            if(fileBytes == null) return;

            var name = Path.GetRandomFileName() + "_" + DateTime.UtcNow.ToString("dd/MM/yyyy").Replace("/", "_");

            var url = await _blobsManagement.UploadFile("images", name, fileBytes, connection);
        }
    }