[System.Serializable]
public class SlotServerException : System.Exception
{
    public SlotServerException() { }
    public SlotServerException(string message) : base(message) { }
    public SlotServerException(string message, System.Exception inner) : base(message, inner) { }
    protected SlotServerException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}