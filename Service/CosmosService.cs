using Azure.Identity;
using Microsoft.Azure.Cosmos;
using SupportApp.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SupportApp.Services;

public class CosmosService
{
    private readonly CosmosClient _client;
    private readonly Container _container;
    
    
    public CosmosService(IConfiguration config)
    {
        var cs          = config["Cosmos:ConnectionString"];
        var endpoint    = config["Cosmos:AccountEndpoint"];
        var dbId        = config["Cosmos:DatabaseId"] ?? "CosmosSupportDB";
        var containerId = config["Cosmos:ContainerId"] ?? "cosmossupport";

        // Konfigurer serializer til at bruge System.Text.Json
        var serializerOptions = new CosmosSerializationOptions
        {
            PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
        };

        var clientOptions = new CosmosClientOptions
        {
            ApplicationName = "CosmosDB-App",
            SerializerOptions = serializerOptions
        };

        // Opret CosmosClient
        _client = !string.IsNullOrWhiteSpace(cs)
            ? new CosmosClient(cs, clientOptions)
            : new CosmosClient(endpoint, new DefaultAzureCredential(), clientOptions);

        _container = _client.GetContainer(dbId, containerId);
    }

    public async Task<SupportMessage> CreateAsync(SupportMessage msg, CancellationToken ct = default)
    {
        // Sørg for Id (Cosmos kræver "id")
        if (string.IsNullOrWhiteSpace(msg.Id))
            msg.Id = Guid.NewGuid().ToString();

        var response = await _container.CreateItemAsync(
            item: msg,
            partitionKey: new PartitionKey(msg.Category), // Bruger category som partition key
            cancellationToken: ct
        );

        return response.Resource;
    }

    public async Task<IReadOnlyList<SupportMessage>> GetAllAsync(CancellationToken ct = default)
    {
        const string sql = "SELECT * FROM c ORDER BY c._ts DESC";
        var iterator = _container.GetItemQueryIterator<SupportMessage>(
            new QueryDefinition(sql),
            requestOptions: new QueryRequestOptions
            {
                MaxItemCount = 100
            }
        );

        var results = new List<SupportMessage>();
        while (iterator.HasMoreResults)
        {
            var page = await iterator.ReadNextAsync(ct);
            results.AddRange(page);
        }
        return results;
    }
}