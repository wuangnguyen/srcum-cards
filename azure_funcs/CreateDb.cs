using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace ScrumCards.Function
{
    public static class CreateDb
    {
        [FunctionName("CreateDb")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, [CosmosDB(ConnectionStringSetting = "scrumcards_DOCUMENT_DB")] DocumentClient client)
        {
            await client.CreateDatabaseIfNotExistsAsync(new Database { Id = Constants.DbName });
            Uri dataBaseUri = UriFactory.CreateDatabaseUri(Constants.DbName);
            await client.CreateDocumentCollectionIfNotExistsAsync(dataBaseUri, new DocumentCollection
            {
                Id = Constants.CollectionName,
                PartitionKey = new PartitionKeyDefinition
                {
                    Paths = new Collection<string> { "/card_item" }
                }
            }, new RequestOptions { OfferThroughput = 400 });
            return new OkResult();
        }
    }
}