using Azure.Storage.Blobs;
using DevForABuck.Application.Interfaces;
using DevForABuck.Domain.Entities;
// using DevForABuck.Domain.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DevForABuck.Infrastructure.Services
{
    public class BookingService : IBookingService
    {
        private readonly Container _container;
        private readonly BlobContainerClient _blobContainerClient;

        public BookingService(CosmosClient cosmosClient, BlobServiceClient blobServiceClient, IConfiguration config)
        {
            var dbName = config["CosmosDb:DatabaseName"];
            var containerName = config["CosmosDb:ContainerName"];
            _container = cosmosClient.GetDatabase(dbName).GetContainer(containerName);

            var blobContainerName = config["BlobStorage:ContainerName"];
            _blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
            _blobContainerClient.CreateIfNotExists();
        }

        public async Task<Booking> CreateBookingAsync(Booking booking, Stream resumeStream, string fileName)
        {
            // Upload to Blob Storage ✅
            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var blobClient = _blobContainerClient.GetBlobClient(uniqueFileName);
            await blobClient.UploadAsync(resumeStream, overwrite: true);
            booking.ResumeUrl = blobClient.Uri.ToString();

            Console.WriteLine("Booking BEFORE Cosmos insert: " + JsonConvert.SerializeObject(booking));

            try
            {
                var response = await _container.CreateItemAsync(booking, new PartitionKey(booking.Email));
                Console.WriteLine("Cosmos insert SUCCESS: " + JsonConvert.SerializeObject(response.Resource));
                return response.Resource;
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Cosmos insert FAILED: {ex.StatusCode} - {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return null;
            }
        }


        public async Task<IEnumerable<Booking>> GetBookingsByEmailAsync(string email)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.email = @email")
                .WithParameter("@email", email);
            
            var iterator = _container.GetItemQueryIterator<Booking>(query);
            var results = new List<Booking>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var iterator = _container.GetItemQueryIterator<Booking>(query);
            var results = new List<Booking>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }
    }
}
