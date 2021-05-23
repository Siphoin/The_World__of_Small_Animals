using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class UserMessage : UserMessageBase, ISeterText, ISeterColor,  IPunObservable
    {
    [Header("Компонент текста")]
  [SerializeField]  private TextMeshProUGUI textData;

    private string textSync = "";

        // Use this for initialization
        void Start()
    {

        Ini();
    }


    public void SetText (string text)
    {
        Ini();

        text = text.Trim();

        textData.text = text;

        if (View.IsMine)
        {
            textSync = text;
        }
    }

    public override void Ini()
    {
        if (textData == null)
        {
            throw new UserMessageException("text object not seted for user text message");
        }
        base.Ini();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(textSync);
            stream.SendNext(isLast);
        }

        else
        {
            textSync = (string)stream.ReceiveNext();
            SetText(textSync);

            isLast = (bool)stream.ReceiveNext();
            CheckCloudMessageisLast();
        }
    }

    public void SetColor(Color color)
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {

        if (!View.IsMine)
        {
            return;
        }


        if (transform.GetSiblingIndex() != transform.parent.childCount - 1)
        {
            isLast = true;
            CheckCloudMessageisLast();
            return;
        }
    }

}