using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TextMeshProUGUI))]
    public class NickNameText : MonoBehaviour, ISeterText
    {
    private const string PATH_DATA_TEXT_CHOOSE_COLOR = "Data/UI/NickName/NickNameChooseColorText";

    [Header("Персонаж")]
    [SerializeField] private PhotonView character;


    private PhotonView view;

    private TextMeshProUGUI textNickName;

    private NickNameChooseColor nickNameChooseColorParams;


    // Use this for initialization
    void Start()
        {
        if (character == null)
        {
            throw new NickNameTextException("parent character not seted");
        }


        if (!TryGetComponent(out textNickName))
        {
            throw new NickNameTextException($"text nickname {name} not have component TextMeshProUGUI");
        }


        if (!character.TryGetComponent(out view))
        {
            throw new NickNameTextException($"parent character {transform.parent.name} not have component Photon View");
        }

        nickNameChooseColorParams = Resources.Load<NickNameChooseColor>(PATH_DATA_TEXT_CHOOSE_COLOR);

        if (nickNameChooseColorParams == null)
        {
            throw new NickNameTextException("params color nickname text not found");
        }

       SetText(view.Owner.NickName);
       SetColorText();


        }

    public void SetText(string text)
    {
        textNickName.text = text;
    }

    private void SetColorText ()
    {
        textNickName.color = view.IsMine ? nickNameChooseColorParams.IsMineColorText : nickNameChooseColorParams.DefaultColorText;
    }

}