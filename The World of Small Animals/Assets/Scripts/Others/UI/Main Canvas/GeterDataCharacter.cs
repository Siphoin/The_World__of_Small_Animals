using System.Collections;
using UnityEngine;

    public class GeterDataCharacter : MonoBehaviour
    {
        public CharacterRequestData CharacterData { get; private set; }

    protected void Ini ()
    {
        if (AuthCharacter.Manager == null)
        {
            throw new GeterDataCharacterException("auth character manager not found");
        }

        if (AuthCharacter.Manager.CharacterData == null)
        {
            throw new GeterDataCharacterException("character data is null or not called auth");
        }

        CharacterData = AuthCharacter.Manager.CharacterData;
    }
    }
