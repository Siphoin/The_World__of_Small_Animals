[System.Serializable]
public class CharacterListException : System.Exception
{
    public CharacterListException() { }
    public CharacterListException(string message) : base(message) { }
    public CharacterListException(string message, System.Exception inner) : base(message, inner) { }
    protected CharacterListException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}