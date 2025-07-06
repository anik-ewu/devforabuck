using Azure.Storage.Blobs;
using DevForABuck.Application.Interfaces;
using DevForABuck.Domain.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace DevForABuck.Infrastructure.Services
{
    public class BookingService: IBookingService
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
            var blobName = $"{Guid.NewGuid()}_{fileName}";
            var blobClient = _blobContainerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(resumeStream);
            booking.ResumeUrl = blobClient.Uri.ToString();

            await _container.CreateItemAsync(booking, new PartitionKey(booking.Email));
            return booking;
        }
        
        public async Task<IEnumerable<Booking>> GetBookingsByEmailAsync(string email)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.Email = @email")
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

