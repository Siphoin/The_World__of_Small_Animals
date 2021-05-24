using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class SmileButtonUI : MonoBehaviour, ISeterSprite, IInstantiateNetworkObject, ISeterListSprites
    {
    private Button button;

    private Image icoSmile;

    [Header("Префаб сообщения со смайлом")]
    [SerializeField] private SmileMessage smileMessagePrefab;

    [ReadOnlyField]
    [Header("Индекс элемента спрайта")]
    [SerializeField] private int indexSpriteOfArraySmiles = 0;


    private Sprite[] listSmiles;


    // Use this for initialization
    void Start()
    {
        Ini();

    }

    private void Ini()
    {

        if (smileMessagePrefab == null)
        {
            throw new SmileButtonUIException("smile message prefab not seted");
        }


        if (button == null)
        {
            if (!TryGetComponent(out button))
            {
                throw new SmileButtonUIException($"{name} not have component UnityEngine.UI");
            }

            button.onClick.AddListener(Select);
        }

        if (icoSmile == null)
        {
            if (!TryGetComponent(out icoSmile))
            {
                throw new SmileButtonUIException($"{name} not have component UnityEngine.Image");
            }
        }
    }

    private void Select()
    {
      GameObject newSmileMessage =  InstantiatePlayerObject(smileMessagePrefab.gameObject, Vector3.zero);

        SmileMessage smileMessage = null;

        if (!newSmileMessage.TryGetComponent(out smileMessage))
        {
            throw new SmileButtonUIException($"{name} not have component SmileMessage");
        }

        smileMessage.SetListSprites(listSmiles);
        smileMessage.SetIndexSprite(indexSpriteOfArraySmiles);
        smileMessage.SetSpriteAtCurrentIndex();
    }

    public void SetSprite(Sprite sprite)
    {

        Ini();


        icoSmile.sprite = sprite;
    }

    public void SetIndexSprite (int index)
    {
        if (index < 0)
        {
            throw new SmileButtonUIException("index smile not be that < 0");
        }

        indexSpriteOfArraySmiles = index;


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

    public void SetListSprites(Sprite[] list)
    {

        Ini();


        if (list == null || list.Length == 0)
        {
            throw new SmileButtonUIException("list smiles or null or length = 0");
        }

        listSmiles = list;
    }
}