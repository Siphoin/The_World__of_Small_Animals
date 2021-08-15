using System;

[Serializable]
 public   class ServerRequestData : MongoSchema
    {
    private string name;

    private string[] players;

    private int _countPlayers;

    public string Name => name;
    public string[] Players => players;
    public int CountPlayers => _countPlayers;

    public ServerRequestData ()
    {

    }
    }
