[System.Serializable]
public class WindowSelectCharacterException : System.Exception
{
    public WindowSelectCharacterException() { }
    public WindowSelectCharacterException(string message) : base(message) { }
    public WindowSelectCharacterException(string message, System.Exception inner) : base(message, inner) { }
    protected WindowSelectCharacterException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}