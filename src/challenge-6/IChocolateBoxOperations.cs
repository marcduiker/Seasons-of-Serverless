using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeasonsOfServerless
{
    public interface IChocolateBoxOperations
    {
        Task Add(string chocolate);
        Task Remove(string chocolate);
        Task Reserve((string name, string chocolate) reservation);
        Task UnReserve(string name);
    }
}