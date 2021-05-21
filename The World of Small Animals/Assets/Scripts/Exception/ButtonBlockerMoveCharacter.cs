[System.Serializable]
public class ButtonBlockerMoveCharacterException : System.Exception
{
    public ButtonBlockerMoveCharacterException() { }
    public ButtonBlockerMoveCharacterException(string message) : base(message) { }
    public ButtonBlockerMoveCharacterException(string message, System.Exception inner) : base(message, inner) { }
    protected ButtonBlockerMoveCharacterException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}