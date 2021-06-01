[System.Serializable]
public class AuthFormException : System.Exception
{
    public AuthFormException() { }
    public AuthFormException(string message) : base(message) { }
    public AuthFormException(string message, System.Exception inner) : base(message, inner) { }
    protected AuthFormException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}