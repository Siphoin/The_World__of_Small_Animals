[System.Serializable]
public class DestroyerAnimatorException : System.Exception
{
    public DestroyerAnimatorException() { }
    public DestroyerAnimatorException(string message) : base(message) { }
    public DestroyerAnimatorException(string message, System.Exception inner) : base(message, inner) { }
    protected DestroyerAnimatorException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}