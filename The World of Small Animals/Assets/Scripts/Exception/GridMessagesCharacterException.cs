[System.Serializable]
public class GridMessagesCharacterException : System.Exception
{
    public GridMessagesCharacterException() { }
    public GridMessagesCharacterException(string message) : base(message) { }
    public GridMessagesCharacterException(string message, System.Exception inner) : base(message, inner) { }
    protected GridMessagesCharacterException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}