using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace SeasonsOfServerless
{
    public static class ReserveChocolateClient
    {
        [FunctionName(nameof(ReserveChocolateClient))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethod.Post),
                Route = "reserve/{name}/{chocolate?}")] HttpRequest req,
            string name,
            string chocolate,
            ILogger log,
            [DurableClient] IDurableEntityClient entityClient
        )
        {
            var entityId = new EntityId(nameof(ChocolateBox), Environment.GetEnvironmentVariable("ChocolateBoxId"));
            await entityClient.SignalEntityAsync<IChocolateBoxOperations>(
                entityId,
                chocolateBox => chocolateBox.Reserve((name, chocolate)));
            var entityResponse = await entityClient.ReadEntityStateAsync<ChocolateBox>(entityId);
            
            return new OkObjectResult(entityResponse.EntityState.ReservedChocolates);
        }
    }
}