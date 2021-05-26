using System;
[Serializable]
 public   class ServerRequestData
    {
    public string name;
    public int countPlayers;

    public ServerRequestData ()
    {

    }

    public ServerRequestData (string name, int countPlayers) {
        this.name = name;
        this.countPlayers = countPlayers;
    }
    }
