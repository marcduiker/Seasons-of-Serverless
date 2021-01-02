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
    public static class ChocolateClient
    {
        [FunctionName(nameof(ChocolateClient))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethod.Post),
                nameof(HttpMethod.Delete),
                Route = "chocolate/{chocolate}")] HttpRequest req,
            string chocolate,
            ILogger log,
            [DurableClient] IDurableClient client
        )
        {
            var entityId = new EntityId(nameof(ChocolateBox), Environment.GetEnvironmentVariable("ChocolateBoxId"));
            
            var input = new OrchestratorInput(Environment.GetEnvironmentVariable("ChocolateBoxId"));
            input.ListMethod = nameof(IChocolateBoxOperations.GetAvailableChocolates);
            input.InputArgument = chocolate;
            if (req.Method.Equals(nameof(HttpMethod.Post), StringComparison.OrdinalIgnoreCase))
            {
                input.OperationMethod = nameof(IChocolateBoxOperations.Add);
            }

            if (req.Method.Equals(nameof(HttpMethod.Delete), StringComparison.OrdinalIgnoreCase))
            {
                input.OperationMethod = nameof(IChocolateBoxOperations.Remove);
            }

            if (!string.IsNullOrEmpty(input.OperationMethod))
            {
                var instanceId = await client.StartNewAsync<OrchestratorInput>(
                    nameof(ChocolateOrchestrator),
                    Guid.NewGuid().ToString(),
                    input);
                
                return client.CreateCheckStatusResponse(req, instanceId);
            }
            else
            {
                return new BadRequestResult();
            }
        }
    }
}