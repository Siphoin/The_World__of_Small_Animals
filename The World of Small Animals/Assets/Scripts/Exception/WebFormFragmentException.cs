[System.Serializable]
public class WebFormFragmentException : System.Exception
{
    public WebFormFragmentException() { }
    public WebFormFragmentException(string message) : base(message) { }
    public WebFormFragmentException(string message, System.Exception inner) : base(message, inner) { }
    protected WebFormFragmentException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}