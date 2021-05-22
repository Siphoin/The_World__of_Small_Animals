[System.Serializable]
public class ElementBlockerMoveCharacterException : System.Exception
{
    public ElementBlockerMoveCharacterException() { }
    public ElementBlockerMoveCharacterException(string message) : base(message) { }
    public ElementBlockerMoveCharacterException(string message, System.Exception inner) : base(message, inner) { }
    protected ElementBlockerMoveCharacterException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}