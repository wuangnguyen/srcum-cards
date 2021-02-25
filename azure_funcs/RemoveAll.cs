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
    public static class RemoveAll
    {
        [FunctionName("RemoveAll")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "reset")] HttpRequest req, [CosmosDB (
                ConnectionStringSetting = "scrumcards_DOCUMENT_DB")] DocumentClient client)
        {
            Uri collectionUri = UriFactory.CreateDocumentCollectionUri(Constants.DbName, Constants.CollectionName);
            await client.DeleteDocumentCollectionAsync(collectionUri);

            await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(Constants.DbName), new DocumentCollection
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