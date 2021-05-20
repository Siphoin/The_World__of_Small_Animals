[System.Serializable]
public class ProgressBarException : System.Exception
{
    public ProgressBarException() { }
    public ProgressBarException(string message) : base(message) { }
    public ProgressBarException(string message, System.Exception inner) : base(message, inner) { }
    protected ProgressBarException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}