[System.Serializable]
public class CacheUserAuthManagerException : System.Exception
{
    public CacheUserAuthManagerException() { }
    public CacheUserAuthManagerException(string message) : base(message) { }
    public CacheUserAuthManagerException(string message, System.Exception inner) : base(message, inner) { }
    protected CacheUserAuthManagerException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}