[System.Serializable]
public class AuthFormBaseException : System.Exception
{
    public AuthFormBaseException() { }
    public AuthFormBaseException(string message) : base(message) { }
    public AuthFormBaseException(string message, System.Exception inner) : base(message, inner) { }
    protected AuthFormBaseException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}