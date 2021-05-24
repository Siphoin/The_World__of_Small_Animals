using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SmileMessage : UserMessageBase, IPunObservable, ISeterSprite, ISeterListSprites
    {

    private const string PATH_SMILES_DATA = "Data/UI/Smiles/SmileContainer";


    private long indexSpriteSmile;

    [Header("Изображение для вывода спрайта")]
    [SerializeField] Image imageSmile;

    private Sprite[] listSmiles = new Sprite[0];

    public long IndexSpriteSmile { get => indexSpriteSmile; set => indexSpriteSmile = value; }

    // Use this for initialization
    void Start()
        {
        Ini();
        }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(indexSpriteSmile);
        }

        else
        {

            if (listSmiles.Length == 0)
            {
                LoadSmilesData();

            }
            indexSpriteSmile = (long)stream.ReceiveNext();

            SetSpriteAtCurrentIndex();
        }
    }

    private void LoadSmilesData()
    {
        SmileContainer smileContainer = Resources.Load<SmileContainer>(PATH_SMILES_DATA);

        if (smileContainer == null)
        {
            throw new UserMessageException("smile container not found");
        }

        listSmiles = smileContainer.SmilesSprites;
    }

    public void SetSprite(Sprite sprite)
    {
        Ini();

        imageSmile.sprite = sprite;
    }

    public void SetListSprites (Sprite[] list)
    {

        Ini();


        if (list == null || list.Length == 0)
        {
            throw new UserMessageException("list smiles or null or length = 0");
        }

        listSmiles = list;
    }

    public override void Ini()
    {
        if (imageSmile == null)
        {
            throw new UserMessageException("image smile not seted");
        }
        base.Ini();
    }

    public void SetSpriteAtCurrentIndex ()
    {
        SetSprite(listSmiles[indexSpriteSmile]);
    }

    private void Update()
    {

        if (!View.IsMine)
        {
            return;
        }

        CheckParamsMessage();
    }

    public override void CheckCloudMessageisLast()
    {
        if (isLast)
        {
            imageSmile.color = GetAlphaColor(imageSmile.color);
        }
        base.CheckCloudMessageisLast();
    }

    public void SetIndexSprite (int index)
    {
        indexSpriteSmile = index;
    }
}