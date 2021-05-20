[System.Serializable]
public class JoinLocationException : System.Exception
{
    public JoinLocationException() { }
    public JoinLocationException(string message) : base(message) { }
    public JoinLocationException(string message, System.Exception inner) : base(message, inner) { }
    protected JoinLocationException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}