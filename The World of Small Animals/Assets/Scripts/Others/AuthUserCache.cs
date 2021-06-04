[System.Serializable]
 public   class AuthUserCache
    {
    public string name = null;
    public string password = null;

    public AuthUserCache ()
    {

    }

    public AuthUserCache(string name, string password)
    {
        this.name = name;
        this.password = password;
    }
    }
