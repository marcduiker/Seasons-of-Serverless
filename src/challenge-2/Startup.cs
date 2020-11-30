using System;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SeasonsOfServerless;

[assembly: FunctionsStartup(typeof(Startup))]
namespace SeasonsOfServerless
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var customVisionEndpoint = Environment.GetEnvironmentVariable("CustomVisionEndPoint");
            var predictionKey = Environment.GetEnvironmentVariable("CustomVisionPredictionKey");
            var apiKeyServiceClientCredentials = new ApiKeyServiceClientCredentials(predictionKey);
            
            var customVisionPredictionClient = new CustomVisionPredictionClient(apiKeyServiceClientCredentials) 
            {
                Endpoint = customVisionEndpoint,
            };

            builder.Services.AddSingleton<ICustomVisionPredictionClient>(customVisionPredictionClient);
        }
    }
}