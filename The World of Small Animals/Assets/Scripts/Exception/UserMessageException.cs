[System.Serializable]
public class UserMessageException : System.Exception
{
    public UserMessageException() { }
    public UserMessageException(string message) : base(message) { }
    public UserMessageException(string message, System.Exception inner) : base(message, inner) { }
    protected UserMessageException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}