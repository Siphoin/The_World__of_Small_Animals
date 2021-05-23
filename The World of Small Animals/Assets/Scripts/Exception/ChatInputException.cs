[System.Serializable]
public class ChatInputException : System.Exception
{
    public ChatInputException() { }
    public ChatInputException(string message) : base(message) { }
    public ChatInputException(string message, System.Exception inner) : base(message, inner) { }
    protected ChatInputException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}