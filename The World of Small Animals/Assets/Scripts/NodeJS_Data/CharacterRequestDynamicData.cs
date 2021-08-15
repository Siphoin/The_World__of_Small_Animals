using System;
[Serializable]
 public   class CharacterRequestDynamicData
    {
    private string lastDate;

    private string[] _friendsList;

    private GiftRequestData[] gifts;

    public string LastDate => lastDate;
    public string[] FriendsList => _friendsList;
    public GiftRequestData[] Gifts => gifts;

    public CharacterRequestDynamicData ()
    {
        
    }
    }
