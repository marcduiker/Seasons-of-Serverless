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
    public static class AddChocolateClient
    {
        [FunctionName(nameof(AddChocolateClient))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethod.Post),
                Route = "add/{chocolate}")] HttpRequest req,
            string chocolate,
            ILogger log,
            [DurableClient] IDurableEntityClient entityClient
        )
        {
            var entityId = new EntityId(nameof(ChocolateBox), Environment.GetEnvironmentVariable("ChocolateBoxId"));
            await entityClient.SignalEntityAsync<IChocolateBoxOperations>(
                entityId,
                chocolateBox => chocolateBox.Add(chocolate));
            var entityResponse = await entityClient.ReadEntityStateAsync<ChocolateBox>(entityId);

            return new OkObjectResult(entityResponse.EntityState.AvailableChocolates);
        }
    }
}