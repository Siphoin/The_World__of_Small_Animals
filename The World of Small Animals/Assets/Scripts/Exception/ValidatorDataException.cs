[System.Serializable]
public class ValidatorDataException : System.Exception
{
    public ValidatorDataException() { }
    public ValidatorDataException(string message) : base(message) { }
    public ValidatorDataException(string message, System.Exception inner) : base(message, inner) { }
    protected ValidatorDataException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}