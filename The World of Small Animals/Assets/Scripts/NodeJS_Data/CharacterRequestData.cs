﻿using System;
[Serializable]
 public   class CharacterRequestData : MongoSchema
    {
    public string dateReg;
    public CharacterRequestDynamicData data;
    public bool online;
    public string name;
    public int prefabIndex;

    public int gems;
    public int anicoins;
    public CharacterRequestData ()
    {

    }
    }