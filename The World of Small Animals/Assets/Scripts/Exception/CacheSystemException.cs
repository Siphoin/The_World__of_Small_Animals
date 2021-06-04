[System.Serializable]
public class CacheSystemException : System.Exception
{
    public CacheSystemException() { }
    public CacheSystemException(string message) : base(message) { }
    public CacheSystemException(string message, System.Exception inner) : base(message, inner) { }
    protected CacheSystemException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}