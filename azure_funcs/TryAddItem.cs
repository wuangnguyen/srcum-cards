using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

namespace ScrumCards.Function
{
    public static class TryAddItem
    {
        [FunctionName("TryAddItem")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "item")] HttpRequest req, [CosmosDB(Constants.DbName, Constants.CollectionName, ConnectionStringSetting = "scrumcards_DOCUMENT_DB", CreateIfNotExists = true)] IAsyncCollector<CardItem> documents)
        {
            string id = req.Headers["id"];
            if (string.IsNullOrEmpty(id))
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            var value = data?.value;
            await documents.AddAsync(new CardItem { Id = id, Value = value });
            return new OkResult();
        }
    }
}