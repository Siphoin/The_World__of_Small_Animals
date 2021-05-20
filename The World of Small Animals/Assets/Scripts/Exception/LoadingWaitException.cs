[System.Serializable]
public class LoadingWaitException : System.Exception
{
    public LoadingWaitException() { }
    public LoadingWaitException(string message) : base(message) { }
    public LoadingWaitException(string message, System.Exception inner) : base(message, inner) { }
    protected LoadingWaitException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}