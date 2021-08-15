using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UserMessage : UserMessageBase, ISeterText, ISeterColor,  IPunObservable
    {
    [Header("Компонент текста")]
  [SerializeField]  private TextMeshProUGUI _textData;

    private string _textSync = "";


    public void SetText (string text)
    {
        Ini();

        text = text.Trim();

        _textData.text = text;

        if (View.IsMine)
        {
            _textSync = text;
        }
    }

    public override void Ini()
    {
        if (_textData == null)
        {
            throw new UserMessageException("text object not seted for user text message");
        }

        base.Ini();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_textSync);
            stream.SendNext(isLast);
            stream.SendNext(indexMessage);
        }

        else
        {
            _textSync = (string)stream.ReceiveNext();

            SetText(_textSync);

            isLast = (bool)stream.ReceiveNext();
            CheckCloudMessageisLast();

            indexMessage = (int)stream.ReceiveNext();

            transform.SetSiblingIndex(indexMessage);
        }
    }

    public void SetColor(Color color) => throw new System.NotImplementedException();

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
            _textData.color = GetAlphaColor(_textData.color);

        }

        base.CheckCloudMessageisLast();
    }

    void Start() => Ini();

}