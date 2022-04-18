using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SmileMessage : UserMessageBase, IPunObservable, ISeterSprite, ISeterListSprites
    {

    private const string PATH_SMILES_DATA = "Data/UI/Smiles/SmileContainer";


    private long _indexSpriteSmile;

    [Header("Изображение для вывода спрайта")]
    [SerializeField] Image _imageSmile;

    private Sprite[] _listSmiles = new Sprite[0];

    public long IndexSpriteSmile { get => _indexSpriteSmile; set => _indexSpriteSmile = value; }

   

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_indexSpriteSmile);
            stream.SendNext(isLast);
            stream.SendNext(indexMessage);

        }

        else
        {

            if (_listSmiles.Length == 0)
            {
                LoadSmilesData();

            }
            _indexSpriteSmile = (long)stream.ReceiveNext();

            isLast = (bool)stream.ReceiveNext();
            
            CheckCloudMessageisLast();

            indexMessage = (int)stream.ReceiveNext();

            transform.SetSiblingIndex(indexMessage);

            SetSpriteAtCurrentIndex();
        }
    }

    private void LoadSmilesData()
    {
        SmileContainer _smileContainer = Resources.Load<SmileContainer>(PATH_SMILES_DATA);

        if (_smileContainer == null)
        {
            throw new UserMessageException("smile container not found");
        }

        _listSmiles = _smileContainer.SmilesSprites;
    }

    public void SetSprite(Sprite sprite)
    {
        Ini();

        _imageSmile.sprite = sprite;
    }

    public void SetListSprites (Sprite[] list)
    {

        Ini();


        if (list == null || list.Length == 0)
        {
            throw new UserMessageException("list smiles or null or length = 0");
        }

        _listSmiles = list;
    }

    public override void Ini()
    {
        if (_imageSmile == null)
        {
            throw new UserMessageException("image smile not seted");
        }
        base.Ini();
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
            _imageSmile.color = GetAlphaColor(_imageSmile.color);
        }
        base.CheckCloudMessageisLast();
    }
    
      private  void Start() => Ini();

    public void SetIndexSprite (int index) =>  _indexSpriteSmile = index;
    
    public void SetSpriteAtCurrentIndex () =>  SetSprite(_listSmiles[_indexSpriteSmile]);
   
}
