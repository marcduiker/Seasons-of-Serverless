using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace SeasonsOfServerless
{
    public static class ListAvailableChocolateClient
    {
        [FunctionName(nameof(ListAvailableChocolateClient))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethod.Get),
                Route = "list")] HttpRequest req,
            ILogger log,
            [DurableClient] IDurableEntityClient entityClient
        )
        {
            var entityId = new EntityId(nameof(ChocolateBox), Environment.GetEnvironmentVariable("ChocolateBoxId"));
            var entityResponse = await entityClient.ReadEntityStateAsync<ChocolateBox>(entityId);
            
            IActionResult result = new BadRequestResult();
            if (entityResponse.EntityExists)
            {
                result = new OkObjectResult(
                    new {
                        available = entityResponse.EntityState.AvailableChocolates,
                        reserved = entityResponse.EntityState.ReservedChocolates
                    });
            }
            else
            {
                result = new BadRequestObjectResult("ChocolateBox is empty. Please add chocolates first.");
            }

            return result;
        }
    }
}