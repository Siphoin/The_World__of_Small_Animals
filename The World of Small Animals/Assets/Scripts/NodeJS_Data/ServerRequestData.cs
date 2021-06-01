using System;
[Serializable]
 public   class ServerRequestData : MongoSchema
    {
    public string name;
    public string[] players;
    public int countPlayers;

    public ServerRequestData ()
    {

    }
    }
