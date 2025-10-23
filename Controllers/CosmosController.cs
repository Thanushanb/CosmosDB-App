using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using SupportApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SupportApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SupportMessagesController : ControllerBase
    {
        private readonly CosmosClient _client;
        private readonly Container _container;

        public SupportMessagesController(IConfiguration config)
        {
            var cs = config["Cosmos:ConnectionString"];
            var endpoint = config["Cosmos:AccountEndpoint"];
            var dbId = config["Cosmos:DatabaseId"] ?? "CosmosSupportDB";
            var containerId = config["Cosmos:ContainerId"] ?? "cosmossupport";

            var cosmosOptions = new CosmosClientOptions 
            { 
                ApplicationName = "CosmosDB-App",
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            };

            if (!string.IsNullOrWhiteSpace(cs))
            {
                _client = new CosmosClient(cs, cosmosOptions);
            }
            else
            {
                _client = new CosmosClient(endpoint, new DefaultAzureCredential(), cosmosOptions);
            }

            _container = _client.GetContainer(dbId, containerId);
        }

        [HttpGet]
        public async Task<IEnumerable<SupportMessage>> Get()
        {
            var results = new List<SupportMessage>();
            const string sql = "SELECT * FROM c ORDER BY c._ts DESC";
            var iterator = _container.GetItemQueryIterator<SupportMessage>(new QueryDefinition(sql));

            while (iterator.HasMoreResults)
            {
                var page = await iterator.ReadNextAsync();
                results.AddRange(page);
            }

            return results;
        }

        [HttpPost]
        public async Task<ActionResult<SupportMessage>> Post([FromBody] SupportMessage msg)
        {
            if (msg == null) return BadRequest();
            if (string.IsNullOrWhiteSpace(msg.Id)) msg.Id = Guid.NewGuid().ToString();
            if (string.IsNullOrWhiteSpace(msg.Category)) return BadRequest("Category (partition key) er påkrævet.");

            var response = await _container.CreateItemAsync(
                item: msg,
                partitionKey: new PartitionKey(msg.Category)
            );

            return CreatedAtAction(nameof(Get), new { id = msg.Id, category = msg.Category }, response.Resource);
        }
    }
}