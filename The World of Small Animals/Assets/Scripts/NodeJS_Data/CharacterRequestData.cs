using System;
using UnityEngine;

[Serializable]
 public   class CharacterRequestData : MongoSchema
    {
    private string dateReg;
    
    private string name;
    
    
    private bool online;
    
    private int prefabIndex;

    private int gems;
    
    private int anicoins;
    
    private CharacterRequestDynamicData data;
    
    public CharacterRequestData ()
    {

    }
    }
