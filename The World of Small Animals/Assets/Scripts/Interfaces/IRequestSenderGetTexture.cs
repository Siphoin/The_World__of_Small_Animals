using UnityEngine;

public interface IRequestSenderGetTexture
    {
    void SendRequest();
    void ReceiveTexture(string id, Texture2D texture, RequestResult requestResult, long responseCode);
}
