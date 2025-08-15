using Azure;
using Azure.Data.Tables;

namespace ABCapp.Models
{
    public class CustomerEntity : ITableEntity
    {
        public string PartitionKey { get; set; } = "Customer";
        public string RowKey { get; set; } // e.g., customer email
        public string Name { get; set; }
        public string Email { get; set; }
        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }
}
