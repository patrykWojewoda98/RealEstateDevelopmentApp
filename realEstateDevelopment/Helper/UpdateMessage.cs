namespace realEstateDevelopment.Helper
{
    public class UpdateMessage
    {
        public string Message { get; set; }
        public int Data { get; set; }

        public UpdateMessage(string message, int data)
        {
            Message = message;
            Data = data;
        }
    }
}
