using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SeasonsOfServerless
{
    public static class Challenge3
    {
        [FunctionName(nameof(Challenge3))]
        public static IActionResult Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethods.Get),
                Route = "recipe/kebab/{kgMeat:double}")] HttpRequest req,
                double kgMeat,
            ILogger log)
        {
            if (kgMeat <= 0)
            {
                return new BadRequestObjectResult("Please provide a positive number for the amount of meat in kilograms (e.g. recipe/kebab/2).");
            }
            else
            {
                var kebabRecipe = new KebabRecipe(kgMeat);

                return new OkObjectResult(kebabRecipe);
            }
        }
    }
}
