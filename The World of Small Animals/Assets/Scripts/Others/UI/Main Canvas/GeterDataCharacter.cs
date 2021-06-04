using System.Collections;
using UnityEngine;

    public class GeterDataCharacter : MonoBehaviour
    {
        public CharacterRequestData CharacterData { get; private set; }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame

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