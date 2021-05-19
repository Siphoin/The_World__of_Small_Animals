[System.Serializable]
public class LoadingException : System.Exception
{
    public LoadingException() { }
    public LoadingException(string message) : base(message) { }
    public LoadingException(string message, System.Exception inner) : base(message, inner) { }
    protected LoadingException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}