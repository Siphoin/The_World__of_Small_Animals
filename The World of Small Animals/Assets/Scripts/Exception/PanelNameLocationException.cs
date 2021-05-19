[System.Serializable]
public class PanelNameLocationException : System.Exception
{
    public PanelNameLocationException() { }
    public PanelNameLocationException(string message) : base(message) { }
    public PanelNameLocationException(string message, System.Exception inner) : base(message, inner) { }
    protected PanelNameLocationException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}