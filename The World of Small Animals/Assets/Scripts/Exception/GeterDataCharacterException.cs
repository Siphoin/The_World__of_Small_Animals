[System.Serializable]
public class GeterDataCharacterException : System.Exception
{
    public GeterDataCharacterException() { }
    public GeterDataCharacterException(string message) : base(message) { }
    public GeterDataCharacterException(string message, System.Exception inner) : base(message, inner) { }
    protected GeterDataCharacterException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}