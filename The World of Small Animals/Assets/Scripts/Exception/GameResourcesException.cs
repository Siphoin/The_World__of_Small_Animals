[System.Serializable]
public class GameResourcesException : System.Exception
{
    public GameResourcesException() { }
    public GameResourcesException(string message) : base(message) { }
    public GameResourcesException(string message, System.Exception inner) : base(message, inner) { }
    protected GameResourcesException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}