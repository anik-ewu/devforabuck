using DevForABuck.Application.Interfaces;
using DevForABuck.Domain.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace DevForABuck.Infrastructure.Services;

public class SlotService: ISlotService
{
    private readonly Container _container;


    public SlotService(CosmosClient cosmosClient, IConfiguration config)
    {
        var dbName = config["CosmosDb:DatabaseName"];
        var containerName = config["CosmosDb:SlotsContainerName"];
        _container = cosmosClient.GetContainer(dbName, containerName);
    }
    
    public async Task<AvailableSlot> CreateSlotAsync(AvailableSlot slot)
    {
        try
        {
            var res = await _container.CreateItemAsync(slot, new PartitionKey(slot.SlotType));
            return res.Resource;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public async Task<IEnumerable<AvailableSlot>> GetAllSlotsAsync()
    {
        var query = new QueryDefinition("SELECT * FROM c");
        var iterator = _container.GetItemQueryIterator<AvailableSlot>(query);
        var results = new List<AvailableSlot>();

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response);
        }
        return results;
    }
}