[System.Serializable]
public class ContainerSelectCharactersException : System.Exception
{
    public ContainerSelectCharactersException() { }
    public ContainerSelectCharactersException(string message) : base(message) { }
    public ContainerSelectCharactersException(string message, System.Exception inner) : base(message, inner) { }
    protected ContainerSelectCharactersException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}