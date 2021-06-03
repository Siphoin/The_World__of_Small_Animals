[System.Serializable]
public class ButtonPageBannerException : System.Exception
{
    public ButtonPageBannerException() { }
    public ButtonPageBannerException(string message) : base(message) { }
    public ButtonPageBannerException(string message, System.Exception inner) : base(message, inner) { }
    protected ButtonPageBannerException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}