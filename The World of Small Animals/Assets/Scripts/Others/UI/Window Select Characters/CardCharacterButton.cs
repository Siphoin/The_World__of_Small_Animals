using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
    public class CardCharacterButton : MonoBehaviour, ISeterText
    {
       private Button button;


    private CharacterRequestData currentData;

    public event Action<CharacterRequestData> onSelect;

    [Header("Аватар")]
    [SerializeField] private AvatarUI avatar;

    [Header("Текст имени карточки")]
    [SerializeField] private TextMeshProUGUI textNameCard;

    [Header("Текст имени карточки по умолчанию")]
    [SerializeField] private string nameCard = "NewCard";

    [Header("Устанавливать имя карточки по умолчанию")]
    [SerializeField] private bool setTextNameCardIsDefault = true;

    public bool SetTextNameCardIsDefault { get => setTextNameCardIsDefault; set => setTextNameCardIsDefault = value; }
    public CharacterRequestData CurrentData { get => currentData; }

    // Use this for initialization
    void Start()
        {
        Ini();
        }

    private void Ini ()
    {

        if (avatar == null)
        {
            throw new CardCharacterButtonException("avatar not seted");
        }

        if (setTextNameCardIsDefault)
        {
            if (textNameCard == null)
            {
                throw new CardCharacterButtonException("text name card not seted");
            }

            SetText(nameCard);
        }

        if (button == null)
        {
            if (!TryGetComponent(out button))
            {
                throw new CardCharacterButtonException($"{name} not have component Button");
            }

            button.onClick.AddListener(Select);
        }

    }

    private void Select()
    {

        if (currentData == null)
        {
            throw new CardCharacterButtonException("current data not seted");
        }


        onSelect?.Invoke(currentData);



    }

    public void SetActiveSelectingCameraAngle (bool status)
    {
        avatar.SetCameraAngle(status ? 4 : 5);

        button.interactable = !status;
    }

    public void SetData (CharacterRequestData data)
    {
        if (data == null)
        {
            throw new CardCharacterButtonException("data character is null");
        }

        currentData = data;

        avatar.SetIndexCharacter(currentData.PrefabIndex);

       if (transform.GetSiblingIndex() == 0)
        {
            Select();
        }
    }

    public void SetText(string text)
    {
        if (textNameCard == null)
        {
            throw new CardCharacterButtonException("text name card not seted");
        }

        textNameCard.text = text;
    }
}