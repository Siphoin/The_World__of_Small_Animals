[System.Serializable]
public class CheckboxWebFormException : System.Exception
{
    public CheckboxWebFormException() { }
    public CheckboxWebFormException(string message) : base(message) { }
    public CheckboxWebFormException(string message, System.Exception inner) : base(message, inner) { }
    protected CheckboxWebFormException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}