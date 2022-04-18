using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TextMeshProUGUI))]
    public class NickNameText : MonoBehaviour, ISeterText
    {
    private const string PATH_DATA_TEXT_CHOOSE_COLOR = "Data/UI/NickName/NickNameChooseColorText";

    [Header("Персонаж")]
    [SerializeField] private PhotonView _character;


    private PhotonView _view;

    private TextMeshProUGUI _textNickName;

    private NickNameChooseColor _nickNameChooseColorParams;


    
  private  void Start()
        {
        if (_character == null)
        {
            throw new NickNameTextException("parent character not seted");
        }


        if (!TryGetComponent(out _textNickName))
        {
            throw new NickNameTextException($"text nickname {name} not have component TextMeshProUGUI");
        }


        if (!_character.TryGetComponent(out _view))
        {
            throw new NickNameTextException($"parent character {transform.parent.name} not have component Photon View");
        }

        _nickNameChooseColorParams = Resources.Load<NickNameChooseColor>(PATH_DATA_TEXT_CHOOSE_COLOR);

        if (_nickNameChooseColorParams == null)
        {
            throw new NickNameTextException("params color nickname text not found");
        }

       SetText(_view.Owner.NickName);
       
       SetColorText();


        }

    public void SetText(string text) => _textNickName.text = text;
    
    private void SetColorText () => _textNickName.color = _view.IsMine ? _nickNameChooseColorParams.IsMineColorText : _nickNameChooseColorParams.DefaultColorText;

}
