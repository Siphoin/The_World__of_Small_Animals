[System.Serializable]
public class WindowPictureBannersException : System.Exception
{
    public WindowPictureBannersException() { }
    public WindowPictureBannersException(string message) : base(message) { }
    public WindowPictureBannersException(string message, System.Exception inner) : base(message, inner) { }
    protected WindowPictureBannersException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
