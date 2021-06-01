using System;
[Serializable]
  public  class UserData : MongoSchema
    {
    public string name;
    public int age;
    public string dateReg;
    public string[] characters;
    public string role;

    public UserData ()
    {

    }
    }
