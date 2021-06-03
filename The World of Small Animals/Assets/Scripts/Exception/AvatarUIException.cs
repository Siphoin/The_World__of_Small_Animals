[System.Serializable]
public class AvatarUIException : System.Exception
{
    public AvatarUIException() { }
    public AvatarUIException(string message) : base(message) { }
    public AvatarUIException(string message, System.Exception inner) : base(message, inner) { }
    protected AvatarUIException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}