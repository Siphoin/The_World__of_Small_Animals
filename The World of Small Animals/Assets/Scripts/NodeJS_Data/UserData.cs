using Newtonsoft.Json;
using System;

[Serializable]
  public  class UserData : MongoSchema
    {
    private string _name;

    private string _dateRegister;

    private string _role;

    private string[] _characters;

    private int _age;

    public string Name => _name;

    [JsonProperty("dateReg")]
    public string DateRegister  => _dateRegister;
    public string Role => _role;
    public string[] Characters => _characters;
    public int Age => _age;

    public UserData ()
    {

    }
    }
