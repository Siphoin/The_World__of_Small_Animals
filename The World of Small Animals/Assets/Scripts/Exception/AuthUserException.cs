[System.Serializable]
public class AuthUserException : System.Exception
{
    public AuthUserException() { }
    public AuthUserException(string message) : base(message) { }
    public AuthUserException(string message, System.Exception inner) : base(message, inner) { }
    protected AuthUserException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}