using Photon.Pun;
using UnityEngine;
[RequireComponent(typeof(PhotonView))]
public class GridMessagesCharacter : MonoBehaviour
    {
    private const string PREFIX_GRID_MESSAGES = "Grid_Messages_";

    private const int MAX_COUNT_USER_MESSAGES_ON_GRID = 3;


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

    private void Update()
    {
        if (character != null)
        {
            if (!character.IsMine)
            {
                return;
            }


            if (transform.childCount > MAX_COUNT_USER_MESSAGES_ON_GRID)
            {
                PhotonView viewMessage = null;

                GameObject targetMessageObject = transform.GetChild(0).gameObject;

                if (!targetMessageObject.TryGetComponent(out viewMessage))
                {
                    throw new GridMessagesCharacterException($"{targetMessageObject.name} not found component PhotonView");
                }

                PhotonNetwork.Destroy(targetMessageObject);
            }
        }
    }

}