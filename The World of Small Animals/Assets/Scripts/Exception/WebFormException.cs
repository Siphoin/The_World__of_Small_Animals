[System.Serializable]
public class WebFormException : System.Exception
{
    public WebFormException() { }
    public WebFormException(string message) : base(message) { }
    public WebFormException(string message, System.Exception inner) : base(message, inner) { }
    protected WebFormException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}