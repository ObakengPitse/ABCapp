using ABCapp.Models;
using Azure.Data.Tables;

namespace ABCapp.Services
{
    public class TableStorageService
    {
        private readonly TableClient _customerTable;
        private readonly TableClient _productTable;

        public TableStorageService(IConfiguration config)
        {
            var conn = config["AzureStorage:TableConnectionString"];
            _customerTable = new TableClient(conn, "Customers");
            _productTable = new TableClient(conn, "Products");

            _customerTable.CreateIfNotExists();
            _productTable.CreateIfNotExists();
        }

        public async Task AddCustomerAsync(CustomerEntity customer) =>
            await _customerTable.AddEntityAsync(customer);

        public async Task AddProductAsync(ProductEntity product) =>
            await _productTable.AddEntityAsync(product);

        public async Task<List<ProductEntity>> GetProductsAsync() =>
            _productTable.Query<ProductEntity>().ToList();

        public async Task<ProductEntity> GetProductByIdAsync(string id)
        {
            try
            {
                var response = await _productTable.GetEntityAsync<ProductEntity>("default", id);
                return response.Value;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<CustomerEntity>> GetCustomersAsync()
        {
            return _customerTable.Query<CustomerEntity>().ToList();
        }

        public async Task<CustomerEntity> GetCustomerByIdAsync(string email)
        {
            try
            {
                var response = await _customerTable.GetEntityAsync<CustomerEntity>("Customer", email);
                return response.Value;
            }
            catch
            {
                return null;
            }
        }

        public async Task UpdateCustomerAsync(CustomerEntity customer)
        {
            await _customerTable.UpdateEntityAsync(customer, customer.ETag, TableUpdateMode.Replace);
        }

        public async Task DeleteCustomerAsync(string email)
        {
            await _customerTable.DeleteEntityAsync("Customer", email);
        }


        public async Task UpdateProductAsync(ProductEntity product) =>
            await _productTable.UpdateEntityAsync(product, product.ETag, TableUpdateMode.Replace);

        public async Task DeleteProductAsync(string id) =>
            await _productTable.DeleteEntityAsync("default", id);
    }
}
