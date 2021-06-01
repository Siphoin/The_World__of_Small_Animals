[System.Serializable]
public class InputWebFormException : System.Exception
{
    public InputWebFormException() { }
    public InputWebFormException(string message) : base(message) { }
    public InputWebFormException(string message, System.Exception inner) : base(message, inner) { }
    protected InputWebFormException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}