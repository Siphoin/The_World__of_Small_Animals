[System.Serializable]
public class LoadingWaitManagerException : System.Exception
{
    public LoadingWaitManagerException() { }
    public LoadingWaitManagerException(string message) : base(message) { }
    public LoadingWaitManagerException(string message, System.Exception inner) : base(message, inner) { }
    protected LoadingWaitManagerException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}