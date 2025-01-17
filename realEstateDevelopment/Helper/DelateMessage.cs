namespace realEstateDevelopment.Helper
{
    public class DeleteMessage
    {
        public string Message { get; set; }
        public int Data { get; set; }

        public DeleteMessage(string message, int data)
        {
            Message = message;
            Data = data;
        }
    }
}
