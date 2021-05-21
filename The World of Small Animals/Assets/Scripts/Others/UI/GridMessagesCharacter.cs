using Photon.Pun;
using UnityEngine;

public class GridMessagesCharacter : MonoBehaviour
    {
    private const string PREFIX_GRID_MESSAGES = "Grid_Messages_";


        [Header("Персонаж")]
        [SerializeField] private PhotonView character;
        // Use this for initialization
        void Awake()
        {
        if (character == null)
        {
            throw new GridMessagesCharacterException("character not seted");
        }

        name = PREFIX_GRID_MESSAGES + character.Owner.NickName;
        }

    }