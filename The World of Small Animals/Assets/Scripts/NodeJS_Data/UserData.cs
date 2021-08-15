using System;
[Serializable]
  public  class UserData : MongoSchema
    {
    public string name;
    
    public string dateReg;
    
    public string role;
    
    public string[] characters;
    
    public int age;

    public UserData ()
    {

    }
    }
