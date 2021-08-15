using UnityEngine;
[CreateAssetMenu(menuName = "Data/Character/Character List", order = 0)]


public class CharactersList : ScriptableObject
    {
    [Header("Список персонажей")]
    [SerializeField] private CharacterController[] _characterList = new CharacterController[1];

    public CharacterController GetCharacter (int index)
    {
        if (index < 0)
        {
            throw new CharacterListException("index nwer that 0");
        }

        if (index > _characterList.Length)
        {
            throw new CharacterListException("index > character list length");
        }

        if (_characterList[index] == null)
        {
            throw new CharacterListException($"character on index {index} is null");
        }

        return _characterList[index];

    }
    }