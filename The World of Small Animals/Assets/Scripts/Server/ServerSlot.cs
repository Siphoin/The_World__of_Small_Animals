using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
    public class ServerSlot : MonoBehaviour, ISeterText, ICheckValidStats, ISeterColor
    {
    private const string PATH_DATA_COLOR_SLOT = "Data/UI/ServerSlot/ServerSlotColorData";


    private ServerSlotData _serverSlotData;

    private Button _button;

    private Image _image;

    private ServerSlotColorData _colorDataSlot;

    public event Action<ServerSlotData> OnClick;

    [Header("Текст названия сервера")]
    [SerializeField] private TextMeshProUGUI _textNameServer;

    [Header("Прогресс заполняемости сервера")]
    [SerializeField] private RadialProgress _radialProgress;


    private void Ini ()
    {
        if (_radialProgress == null)
        {
            throw new SlotServerException("radial progress not seted");
        }

        _colorDataSlot = Resources.Load<ServerSlotColorData>(PATH_DATA_COLOR_SLOT);

        if (_colorDataSlot == null)
        {
            throw new SlotServerException("color data slot not found");
        }

        if (_image == null)
        {
            if (!TryGetComponent(out _image))
            {
                throw new SlotServerException($"{name} not have component UnityEngine.UI.Image");
            }
        }


        if (_button == null)
        {
            if (!TryGetComponent(out _button))
            {
                throw new SlotServerException($"{name} not have component UnityEngine.UI.Button");
            }

            _button.onClick.AddListener(Select);
        }


    }

    private void Select()
    {
        if (_serverSlotData == null)
        {
            throw new SlotServerException($"{name} not have data server slot");
        }

        OnClick?.Invoke(_serverSlotData);
    }

    public void SetText(string text)
    {
        Ini();

        if (_serverSlotData == null)
        {
            throw new SlotServerException($"{name} not have data server slot");
        }

        _textNameServer.text = text;
    }



    public void SetData (ServerSlotData data)
    {
        Ini();


        if (data == null)
        {
            throw new SlotServerException($"server slot dataz argument is null");
        }

        try
        {
            
        }
        catch (ValidatorDataException)
        {

            throw;
        }

        _serverSlotData = data;

        CheckValidStats();

        SetText($"{transform.GetSiblingIndex() + 1}. {_serverSlotData.NameServer}");


       
    }

    public void SetOccupancyRate (float value)
    {
        _radialProgress.UpdateProgress(value / (float)_serverSlotData.MaxCountPlayers);

        if (value == _serverSlotData.MaxCountPlayers)
        {
            _button.interactable = false;
            SetColor(_colorDataSlot.BlockedButtonColor);
        }
    }

    public void CheckValidStats()
    {
        foreach (var prop in _serverSlotData.GetType().GetFields())
        {
            try
            {
                ValidatorData.CheckValidFieldStats(prop, _serverSlotData);
            }
            catch (ValidatorDataException)
            {

                throw;
            }
        }

        foreach (var prop in _serverSlotData.GetType().GetProperties())
        {
            try
            {
                ValidatorData.CheckValidPropertyStats(prop, _serverSlotData);
            }
            catch (ValidatorDataException)
            {

                throw;
            }
        }
    }

    void Start() => Ini();

    public void SetColor(Color color) => _image.color = color;
}