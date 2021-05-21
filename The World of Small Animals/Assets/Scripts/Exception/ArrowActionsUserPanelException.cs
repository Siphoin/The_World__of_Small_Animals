[System.Serializable]
public class ArrowActionsUserPanelException : System.Exception
{
    public ArrowActionsUserPanelException() { }
    public ArrowActionsUserPanelException(string message) : base(message) { }
    public ArrowActionsUserPanelException(string message, System.Exception inner) : base(message, inner) { }
    protected ArrowActionsUserPanelException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}