[System.Serializable]
public   struct TokenString
    {
    private string _token;

    public string Token => _token;

    public TokenString (string token)
    {
        _token = token;
    }

}
