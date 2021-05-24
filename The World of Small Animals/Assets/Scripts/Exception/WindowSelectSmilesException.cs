[System.Serializable]
public class WindowSelectSmilesException : System.Exception
{
    public WindowSelectSmilesException() { }
    public WindowSelectSmilesException(string message) : base(message) { }
    public WindowSelectSmilesException(string message, System.Exception inner) : base(message, inner) { }
    protected WindowSelectSmilesException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
