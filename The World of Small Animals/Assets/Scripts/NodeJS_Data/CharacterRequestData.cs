using System;
[Serializable]
 public   class CharacterRequestData : MongoSchema
    {
    public string dateReg;
    
    public string name;
    
    
    public bool online;
    
    public int prefabIndex;

    public int gems;
    
    public int anicoins;
    
    public CharacterRequestDynamicData data;
    
    public CharacterRequestData ()
    {

    }
    }
