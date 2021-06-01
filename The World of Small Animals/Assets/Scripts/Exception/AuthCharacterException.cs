[System.Serializable]
public class AuthCharacterException : System.Exception
{
    public AuthCharacterException() { }
    public AuthCharacterException(string message) : base(message) { }
    public AuthCharacterException(string message, System.Exception inner) : base(message, inner) { }
    protected AuthCharacterException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}