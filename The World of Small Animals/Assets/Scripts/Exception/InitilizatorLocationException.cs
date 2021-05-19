[System.Serializable]
public class InitilizatorLocationException : System.Exception
{
    public InitilizatorLocationException() { }
    public InitilizatorLocationException(string message) : base(message) { }
    public InitilizatorLocationException(string message, System.Exception inner) : base(message, inner) { }
    protected InitilizatorLocationException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}