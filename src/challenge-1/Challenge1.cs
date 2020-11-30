using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;

namespace SeasonsOfServerless
{
    public static class Challenge1
    {
        [FunctionName(nameof(Challenge1))]
        public static IActionResult Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethods.Get),
                Route = "recipe/turkey/{weight:double}")] HttpRequest req,
            double weight)
        {
            if (weight > 0)
            {
                var recipe = new Recipe(weight);
                return new OkObjectResult(recipe);
            }
            else
            {
                return new BadRequestObjectResult("Please don't weigh the turkey in outer space or in negative gravity. Provide a valid weight in lbs.");
            }
        }
    }

    public class Recipe
    {
        public Recipe(double turkeyWeightInLbs)
        {
            Salt = $"{Math.Round(turkeyWeightInLbs * 0.05, 1)} cups";
            Water = $"{Math.Round(turkeyWeightInLbs * 0.66, 1)} gallons";
            BrownSugar = $"{Math.Round(turkeyWeightInLbs * 0.13, 1)} cups";
            Shallots = $"{Math.Round(turkeyWeightInLbs * 0.2, 1)} pieces";
            GarlicCloves = $"{Math.Round(turkeyWeightInLbs * 0.4, 1)} cloves";
            WholePeppercorns = $"{Math.Round(turkeyWeightInLbs * 0.13, 1)} tablespoons";
            DriedUniperBerries = $"{Math.Round(turkeyWeightInLbs * 0.13, 1)} tablespoons";
            FreshRosemary = $"{Math.Round(turkeyWeightInLbs * 0.13, 1)} tablespoons";
            Thyme = $"{Math.Round(turkeyWeightInLbs * 0.06, 1)} tablespoons";
            BrineTime = $"{Math.Round(turkeyWeightInLbs * 2.4, 1)} hours";
            RoastTime = $"{Math.Round(turkeyWeightInLbs * 15, 1)} minutes";
        }

        public string Salt { get; set; }
        public string Water { get; set; }
        public string GarlicCloves { get; set; }
        public string BrownSugar { get; set; }
        public string Shallots { get; set; }
        public string WholePeppercorns { get; set; }
        public string DriedUniperBerries { get; set; }
        public string FreshRosemary { get; set; }
        public string Thyme { get; set; }
        public string BrineTime { get; set; }
        public string RoastTime { get; set; }
    }
}
