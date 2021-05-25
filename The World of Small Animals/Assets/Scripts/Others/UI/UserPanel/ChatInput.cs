using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(TMP_InputField))]
    public class ChatInput : MonoBehaviour, IInstantiateNetworkObject, ISeterText
    {

    private const string PATH_DATA_PARAMS_USER_MESSAGE = "Params/UserMessageParams";

    private UserMessageParams userMessageParams;

    private TMP_InputField input;


    [Header("Кнопка отправки сообщения")]
    [SerializeField] Button button;

    [Header("Префаб облака пользовательского сообщения")]
    [SerializeField] UserMessage userMessagePrefab;



    // Use this for initialization
    void Start()
    {
        Ini();

        input.onEndEdit.AddListener(TrimingInput);

        button.onClick.AddListener(SendUserMessage);

    }

    private void Ini()
    {
        if (button == null)
        {
            throw new ChatInputException("button send message not seted");
        }

        if (userMessagePrefab == null)
        {
            throw new ChatInputException("user message prefab not seted");
        }

        if (userMessageParams == null)
        {
            userMessageParams = Resources.Load<UserMessageParams>(PATH_DATA_PARAMS_USER_MESSAGE);

            if (userMessageParams == null)
            {
                throw new ChatInputException("user message params not found");
            }

            if (userMessageParams.MaxCountSymbolsInput < 0)
            {
                throw new ChatInputException("max count symbols in input not that newer that 0");
            }

        }

        if (input == null)
        {
            if (!TryGetComponent(out input))
            {
                throw new ChatInputException($"{name} not have component TMP InputField");
            }
        }

        input.characterLimit = userMessageParams.MaxCountSymbolsInput;
    }

    private void TrimingInput(string arg0)
    {
        SetText(input.text.Trim());
    }

    private void RemoveTextAtInput ()
    {
        SetText(string.Empty);
    }

    public void SetText(string text)
    {
        Ini();
        input.text = text;
    }

    private void SendUserMessage()
    {
        if (!string.IsNullOrEmpty(input.text))
        {
            TrimingInput(input.text);

         GameObject messageObject = InstantiatePlayerObject(userMessagePrefab.gameObject, userMessagePrefab.transform.position);

            UserMessage newUserMessage = null;

            if (!messageObject.TryGetComponent(out newUserMessage))
            {
                throw new ChatInputException($"{messageObject.name} not have component UserMessage");
            }
            newUserMessage.transform.localScale = Vector3.one;


            newUserMessage.SetText(input.text);
            RemoveTextAtInput();
        }


    }



    public GameObject InstantiatePlayerObject(GameObject gameObject, Vector3 position, Quaternion rotation, byte group = 0, object[] data = null)
    {
        return PhotonNetwork.Instantiate(gameObject.name, position, rotation, group, data);
    }

    public GameObject InstantiateSceneObject(GameObject gameObject, Vector3 position, Quaternion rotation, byte group = 0, object[] data = null)
    {
        return PhotonNetwork.InstantiateRoomObject(gameObject.name, position, rotation, group, data);
    }

    public GameObject InstantiatePlayerObject(GameObject gameObject, Vector3 position, byte group = 0, object[] data = null)
    {
        return PhotonNetwork.Instantiate(gameObject.name, position, Quaternion.identity, group, data);
    }

    public GameObject InstantiateSceneObject(GameObject gameObject, Vector3 position, byte group = 0, object[] data = null)
    {
        return PhotonNetwork.InstantiateRoomObject(gameObject.name, position, Quaternion.identity, group, data);
    }

}