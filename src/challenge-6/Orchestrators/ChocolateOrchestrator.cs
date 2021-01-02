using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace SeasonsOfServerless
{
    public class ChocolateOrchestrator
    {
        [FunctionName(nameof(ChocolateOrchestrator))]
        public static async Task<List<string>> Run(
            [OrchestrationTrigger] IDurableOrchestrationContext context
        )
        {
            var input = context.GetInput<OrchestratorInput>();
            var entityId = new EntityId(nameof(ChocolateBox), input.ChocolateBoxId );
            await context.CallEntityAsync(entityId, input.OperationMethod, input.InputArgument);
            var listResult = await context.CallEntityAsync<List<string>>(entityId, input.ListMethod);

            return listResult;
        }
    }
}