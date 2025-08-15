using Azure.Storage.Blobs;

namespace ABCapp.Services
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _container;

        public BlobStorageService(IConfiguration config)
        {
            var conn = config["AzureStorage:BlobConnectionString"];
            var containerName = config["AzureStorage:BlobContainerName"];
            _container = new BlobContainerClient(conn, containerName);
            _container.CreateIfNotExists();
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            var blobClient = _container.GetBlobClient(file.FileName);
            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, true);
            return blobClient.Uri.ToString();
        }
    }

}
