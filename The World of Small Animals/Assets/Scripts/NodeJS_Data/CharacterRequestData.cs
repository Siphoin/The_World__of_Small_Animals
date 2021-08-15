using Newtonsoft.Json;
using System;

[Serializable]
 public   class CharacterRequestData : MongoSchema
    {
    private string _dateRegister;
    
    private string _name;
    
    
    private bool _online;
    
    private int _prefabIndex;

    private int _gems;
    
    private int _anicoins;
    
    private CharacterRequestDynamicData _data;


    [JsonProperty("dateReg")] public string DateRegister => _dateRegister;
    public string Name  => _name;
    public bool Online => _online;
    public int PrefabIndex =>  _prefabIndex; 
    public int Gems => _gems;
    public int Anicoins => _anicoins;
    public CharacterRequestDynamicData Data => _data;

    public CharacterRequestData ()
    {

    }

}
