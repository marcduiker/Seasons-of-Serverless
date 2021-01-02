namespace SeasonsOfServerless
{
    public class OrchestratorInput
    {
        public OrchestratorInput(string chocolateBoxId)
        {
            ChocolateBoxId = chocolateBoxId;
        }
        
        public string OperationMethod { get; set; }

        public string ListMethod { get; set; }

        public object InputArgument { get; set; }

        public string ChocolateBoxId { get; set; }
    }
}