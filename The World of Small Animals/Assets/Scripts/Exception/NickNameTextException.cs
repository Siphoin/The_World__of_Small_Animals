[System.Serializable]
public class NickNameTextException : System.Exception
{
    public NickNameTextException() { }
    public NickNameTextException(string message) : base(message) { }
    public NickNameTextException(string message, System.Exception inner) : base(message, inner) { }
    protected NickNameTextException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}