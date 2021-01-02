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
    public static class ChocolateReservationClient
    {
        [FunctionName(nameof(ChocolateReservationClient))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethod.Post),
                nameof(HttpMethod.Delete),
                Route = "reservation/{name}/{chocolate?}")] HttpRequest req,
            string name,
            string chocolate,
            ILogger log,
            [DurableClient] IDurableClient client
        )
        {
            var entityId = new EntityId(nameof(ChocolateBox), Environment.GetEnvironmentVariable("ChocolateBoxId"));
            
            var input = new OrchestratorInput(Environment.GetEnvironmentVariable("ChocolateBoxId"));
            input.ListMethod = nameof(IChocolateBoxOperations.GetReservedChocolates);
            if (req.Method.Equals(nameof(HttpMethod.Post), StringComparison.OrdinalIgnoreCase))
            {
                input.OperationMethod = nameof(IChocolateBoxOperations.Reserve);
                input.InputArgument = (name, chocolate);
            }

            if (req.Method.Equals(nameof(HttpMethod.Delete), StringComparison.OrdinalIgnoreCase))
            {
                input.OperationMethod = nameof(IChocolateBoxOperations.UnReserve);
                input.InputArgument = name;
            }

            if (!string.IsNullOrEmpty(input.OperationMethod))
            {
                var instanceId = await client.StartNewAsync<OrchestratorInput>(
                    nameof(ChocolateReservationOrchestrator),
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