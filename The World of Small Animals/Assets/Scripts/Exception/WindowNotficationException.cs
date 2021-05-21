[System.Serializable]
public class WindowNotficationException : System.Exception
{
    public WindowNotficationException() { }
    public WindowNotficationException(string message) : base(message) { }
    public WindowNotficationException(string message, System.Exception inner) : base(message, inner) { }
    protected WindowNotficationException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}