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

        public async Task UpdateProductAsync(ProductEntity product) =>
            await _productTable.UpdateEntityAsync(product, product.ETag, TableUpdateMode.Replace);

        public async Task DeleteProductAsync(string id) =>
            await _productTable.DeleteEntityAsync("default", id);
    }
}
