[System.Serializable]
public class ColorVariantsException : System.Exception
{
    public ColorVariantsException() { }
    public ColorVariantsException(string message) : base(message) { }
    public ColorVariantsException(string message, System.Exception inner) : base(message, inner) { }
    protected ColorVariantsException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}