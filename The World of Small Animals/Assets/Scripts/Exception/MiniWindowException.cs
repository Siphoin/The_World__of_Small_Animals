[System.Serializable]
public class MiniWindowException : System.Exception
{
    public MiniWindowException() { }
    public MiniWindowException(string message) : base(message) { }
    public MiniWindowException(string message, System.Exception inner) : base(message, inner) { }
    protected MiniWindowException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}