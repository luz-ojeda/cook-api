using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace CookApi.Services;

public class AzureService
{
    private readonly string containerName = "recipes";
    private readonly IConfiguration _configuration;

    public AzureService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task DeleteBlobsInFolderAsync(string folderPath)
    {

        string connectionString = _configuration["AzureStorageConnectionString"];
        BlobServiceClient client = new(connectionString);
        BlobContainerClient containerClient = client.GetBlobContainerClient(containerName);
        await foreach (BlobItem blobItem in containerClient.GetBlobsAsync(prefix: folderPath))
        {
            BlobClient blobClient = containerClient.GetBlobClient(blobItem.Name);
            await blobClient.DeleteIfExistsAsync();
        }
    }
}