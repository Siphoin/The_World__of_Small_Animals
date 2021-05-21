[System.Serializable]
public class ManagerWindowsNotficationsException : System.Exception
{
    public ManagerWindowsNotficationsException() { }
    public ManagerWindowsNotficationsException(string message) : base(message) { }
    public ManagerWindowsNotficationsException(string message, System.Exception inner) : base(message, inner) { }
    protected ManagerWindowsNotficationsException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}