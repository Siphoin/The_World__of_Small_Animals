[System.Serializable]
public class CharacterException : System.Exception
{
    public CharacterException() { }
    public CharacterException(string message) : base(message) { }
    public CharacterException(string message, System.Exception inner) : base(message, inner) { }
    protected CharacterException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}