using Azure;
using Azure.Data.Tables;


namespace ABCapp.Models
{
    public class ProductEntity : ITableEntity
    {
        public string PartitionKey { get; set; } = "Product";
        public string RowKey { get; set; } // e.g., product ID
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }

}
