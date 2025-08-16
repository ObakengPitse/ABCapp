using Azure;
using Azure.Data.Tables;

namespace ABCapp.Models
{
    
    public class CustomerEntity : ITableEntity
    {
        // PartitionKey can remain fixed for simplicity, or represent customer type/region/etc.
        public string PartitionKey { get; set; } = "Customer";

        // Use a unique ID for RowKey
        public string RowKey { get; set; } // e.g., customer ID or GUID

        // Regular customer properties
        public string Name { get; set; }

        // Email is now just a regular field, editable
        public string Email { get; set; }

        // Required by ITableEntity
        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }
}
