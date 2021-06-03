[System.Serializable]
public class CardCharacterButtonException : System.Exception
{
    public CardCharacterButtonException() { }
    public CardCharacterButtonException(string message) : base(message) { }
    public CardCharacterButtonException(string message, System.Exception inner) : base(message, inner) { }
    protected CardCharacterButtonException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}