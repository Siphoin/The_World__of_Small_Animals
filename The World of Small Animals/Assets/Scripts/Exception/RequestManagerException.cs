[System.Serializable]
public class RequestManagerException : System.Exception
{
    public RequestManagerException() { }
    public RequestManagerException(string message) : base(message) { }
    public RequestManagerException(string message, System.Exception inner) : base(message, inner) { }
    protected RequestManagerException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}