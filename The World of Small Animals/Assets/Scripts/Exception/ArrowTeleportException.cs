[System.Serializable]
public class ArrowTeleportException : System.Exception
{
    public ArrowTeleportException() { }
    public ArrowTeleportException(string message) : base(message) { }
    public ArrowTeleportException(string message, System.Exception inner) : base(message, inner) { }
    protected ArrowTeleportException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}