[System.Serializable]
public class PanelValuteException : System.Exception
{
    public PanelValuteException() { }
    public PanelValuteException(string message) : base(message) { }
    public PanelValuteException(string message, System.Exception inner) : base(message, inner) { }
    protected PanelValuteException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}