using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace ScrumCards.Function
{
    public static class GetAll
    {
        [FunctionName("GetAll")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "items")] HttpRequest req, [CosmosDB (Constants.DbName, Constants.CollectionName,
                ConnectionStringSetting = "scrumcards_DOCUMENT_DB",
                SqlQuery = "select * from CardItems")] IEnumerable<CardItem> items)
        {
            return new OkObjectResult(items);
        }
    }
}