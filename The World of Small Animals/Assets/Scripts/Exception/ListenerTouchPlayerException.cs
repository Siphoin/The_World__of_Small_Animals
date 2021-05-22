[System.Serializable]
public class ListenerTouchPlayerException : System.Exception
{
    public ListenerTouchPlayerException() { }
    public ListenerTouchPlayerException(string message) : base(message) { }
    public ListenerTouchPlayerException(string message, System.Exception inner) : base(message, inner) { }
    protected ListenerTouchPlayerException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}