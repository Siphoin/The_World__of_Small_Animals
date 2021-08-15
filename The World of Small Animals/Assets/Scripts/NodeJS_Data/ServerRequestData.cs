using System;
[Serializable]
 public   class ServerRequestData : MongoSchema
    {
    public string name;
    
    public int countPlayers;
    
    public string[] players;

    public ServerRequestData ()
    {

    }
    }
