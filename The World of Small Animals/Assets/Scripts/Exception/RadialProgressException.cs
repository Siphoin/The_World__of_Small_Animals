 [System.Serializable]
public class RadialProgressException : System.Exception
{
    public RadialProgressException() { }
    public RadialProgressException(string message) : base(message) { }
    public RadialProgressException(string message, System.Exception inner) : base(message, inner) { }
    protected RadialProgressException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}