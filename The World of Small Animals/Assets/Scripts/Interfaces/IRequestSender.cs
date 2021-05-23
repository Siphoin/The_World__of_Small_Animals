public    interface IRequestSender
    {
    void SendRequest();
    void ReceiveRequest(string id, string text, RequestResult requestResult, long responseCode);
    }
