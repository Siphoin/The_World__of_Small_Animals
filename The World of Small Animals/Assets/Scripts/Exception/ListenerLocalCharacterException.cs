[System.Serializable]
public class ListenerLocalCharacterException : System.Exception
{
    public ListenerLocalCharacterException() { }
    public ListenerLocalCharacterException(string message) : base(message) { }
    public ListenerLocalCharacterException(string message, System.Exception inner) : base(message, inner) { }
    protected ListenerLocalCharacterException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}