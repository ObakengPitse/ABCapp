using ABCapp.Models;
using ABCapp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ABCapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly TableStorageService _tableService;
        private readonly BlobStorageService _blobService;

        public HomeController(TableStorageService tableService, BlobStorageService blobService)
        {
            _tableService = tableService;
            _blobService = blobService;
        }

        // GET: Home/Index
        public async Task<IActionResult> Index()
        {
            var products = await _tableService.GetProductsAsync();
            return View(products);
        }

        // GET: Home/Upload
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        // POST: Home/Upload
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile image, string name, string description)
        {
            if (image != null && image.Length > 0)
            {
                var imageUrl = await _blobService.UploadImageAsync(image);

                var product = new ProductEntity
                {
                    RowKey = Guid.NewGuid().ToString(),
                    Name = name,
                    Description = description,
                    ImageUrl = imageUrl
                };

                await _tableService.AddProductAsync(product);
            }

            return RedirectToAction("Index");
        }

        // GET: Home/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var product = await _tableService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // POST: Home/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(string id, IFormFile image, string name, string description)
        {
            var product = await _tableService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            product.Name = name;
            product.Description = description;

            if (image != null && image.Length > 0)
            {
                product.ImageUrl = await _blobService.UploadImageAsync(image);
            }

            await _tableService.UpdateProductAsync(product);

            return RedirectToAction("Index");
        }

        // POST: Home/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await _tableService.DeleteProductAsync(id);
            return RedirectToAction("Index");
        }
    }
}
