using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using System.Linq;

namespace SeasonsOfServerless
{
    public class Challenge2
    {
        private readonly ICustomVisionPredictionClient _customVisionPredictionClient;

        public Challenge2(ICustomVisionPredictionClient customVisionPredictionClient)
         {
            _customVisionPredictionClient = customVisionPredictionClient;
        }
        
        [FunctionName(nameof(Challenge2))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getprediction")] HttpRequest req,
            ILogger log)
        {
            string imgLink = req.Query["img"];

            if (IsValidImgLink(imgLink))
            {
                var imageUrl = new ImageUrl(imgLink);
                var projectId = Guid.Parse(Environment.GetEnvironmentVariable("CustomVisionProjectId"));
                var publishedName = Environment.GetEnvironmentVariable("CustomVisionPublishedName");
                var imagePrediction = await _customVisionPredictionClient.ClassifyImageUrlAsync(
                    projectId,
                    publishedName,
                    imageUrl);

                var highestPrediction = imagePrediction.Predictions
                    .OrderByDescending(prediction => prediction.Probability)
                    .First();
                var message = $"This looks like {highestPrediction.TagName}! I'm {highestPrediction.Probability} certain of it 🤓.";

                return new OkObjectResult(message);
            }
            else
            {
                return new BadRequestObjectResult("Please provide an image url (png/jp(e)g/gif) in the `img` query parameter.");
            }
        }

        public static bool IsValidImgLink(string imgLink)
        {
            return !string.IsNullOrEmpty(imgLink) && 
                (imgLink.EndsWith(".png") ||
                imgLink.EndsWith(".jpg") || 
                imgLink.EndsWith(".jpeg") ||
                imgLink.EndsWith(".gif"));
        }
    }
}
