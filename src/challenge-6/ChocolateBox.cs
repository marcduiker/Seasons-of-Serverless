using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;

namespace SeasonsOfServerless
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ChocolateBox : IChocolateBoxOperations
    {
        public ChocolateBox()
        {
            if (AvailableChocolates == null)
            {
                AvailableChocolates = new List<string>();
            }
        }
        
        [JsonProperty("availableChocolates")]
        public List<string> AvailableChocolates { get; set; }

        [JsonProperty("reservedChocolates")]
        public Dictionary<string, string> ReservedChocolates { get; set; }

        public Task<List<string>> ListAvailableChocolates()
        {
            return Task.FromResult(AvailableChocolates);
        }

        public Task Add(string chocolate)
        {
            if (!AvailableChocolates.Contains(chocolate))
            {
                AvailableChocolates.Add(chocolate);
            }

            return Task.CompletedTask;
        }

        public Task Remove(string chocolate)
        {
            if (AvailableChocolates.Contains(chocolate))
            {
                AvailableChocolates.Remove(chocolate);
            }

            return Task.CompletedTask;
        }

        public Task Reserve((string name,string chocolate) reservation)
        {
            var chocolate = reservation.chocolate;
            if (string.IsNullOrEmpty(chocolate))
            {
                // Randomly select one of the available chocolates
                var rnd = new Random();
                int randomIndex =  rnd.Next(AvailableChocolates.Count-1);
                chocolate = AvailableChocolates[randomIndex];
            }

            if (AvailableChocolates.Contains(chocolate))
            {
                AvailableChocolates.Remove(chocolate);
                ReservedChocolates.Add(reservation.name, chocolate);
            }

            return Task.CompletedTask;
        }

        public Task UnReserve(string name)
        {
            if (ReservedChocolates.TryGetValue(name, out string chocolate))
            {
                ReservedChocolates.Remove(name);
                AvailableChocolates.Add(chocolate);
            }

            return Task.CompletedTask;

        }

        [FunctionName(nameof(ChocolateBox))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx) 
            => ctx.DispatchAsync<ChocolateBox>();
    }
}