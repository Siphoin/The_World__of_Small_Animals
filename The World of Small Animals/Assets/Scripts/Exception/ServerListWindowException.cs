[System.Serializable]
public class ServerListWindowException : System.Exception
{
    public ServerListWindowException() { }
    public ServerListWindowException(string message) : base(message) { }
    public ServerListWindowException(string message, System.Exception inner) : base(message, inner) { }
    protected ServerListWindowException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
