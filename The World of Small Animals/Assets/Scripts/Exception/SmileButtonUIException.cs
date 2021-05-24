[System.Serializable]
public class SmileButtonUIException : System.Exception
{
    public SmileButtonUIException() { }
    public SmileButtonUIException(string message) : base(message) { }
    public SmileButtonUIException(string message, System.Exception inner) : base(message, inner) { }
    protected SmileButtonUIException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}