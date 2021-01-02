using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace SeasonsOfServerless
{
    public class ChocolateReservationOrchestrator
    {
        [FunctionName(nameof(ChocolateReservationOrchestrator))]
        public static async Task<Dictionary<string, string>> Run(
            [OrchestrationTrigger] IDurableOrchestrationContext context
        )
        {
            var input = context.GetInput<OrchestratorInput>();
            var entityId = new EntityId(nameof(ChocolateBox), input.ChocolateBoxId );
            context.SignalEntity(entityId, input.OperationMethod, input.InputArgument);
            var listResult = await context.CallEntityAsync<Dictionary<string, string>>(entityId, input.ListMethod);

            return listResult;
        }
    }
}