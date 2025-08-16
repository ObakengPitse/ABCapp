using ABCapp.Models;
using ABCapp.Services;
using Azure;
using Microsoft.AspNetCore.Mvc;

namespace ABCapp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly TableStorageService _tableService;

        public CustomerController(TableStorageService tableService)
        {
            _tableService = tableService;
        }

        // GET: /Customer
        public async Task<IActionResult> Index()
        {
            var customers = await _tableService.GetCustomersAsync();
            return View(customers);
        }

        // GET: /Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name, string email)
        {
            

            var customer = new CustomerEntity
            {
                RowKey = Guid.NewGuid().ToString(),
                Name = name,
                Email = email,
                PartitionKey = "Customer"
            };

            await _tableService.AddCustomerAsync(customer);
           
            return RedirectToAction("Index");
        }

        // GET: /Customer/Edit/{email}
        public async Task<IActionResult> Edit(string id)
        {
            var customer = await _tableService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound();
            return View(customer);
        }

        // POST: /Customer/Edit/{email}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [FromForm] string ETagString, CustomerEntity customer)
        {
            if (id != customer.RowKey)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(customer);

            customer.PartitionKey = "Customer";

            // Fix the ETag binding manually
            if (!string.IsNullOrWhiteSpace(ETagString))
            {
                customer.ETag = new Azure.ETag(ETagString);
            }
            else
            {
                // Optional fallback if ETag is missing
                customer.ETag = Azure.ETag.All;
            }

            try
            {
                await _tableService.UpdateCustomerAsync(customer);
                return RedirectToAction(nameof(Index));
            }
            catch (RequestFailedException ex)
            {
                ModelState.AddModelError("", "Error updating customer: " + ex.Message);
                return View(customer);
            }
        }



        // GET: /Customer/Delete/{email}
        public async Task<IActionResult> Delete(string id)
        {
            var customer = await _tableService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound();
            return View(customer);
        }

        // POST: /Customer/Delete/{email}
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _tableService.DeleteCustomerAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
