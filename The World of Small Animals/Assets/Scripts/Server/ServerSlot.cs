using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
    public class ServerSlot : MonoBehaviour, ISeterText, ICheckValidStats
    {
    private ServerSlotData serverSlotData;

    private Button button;

    public event Action<ServerSlotData> onClick;

    [Header("Текст названия сервера")]
    [SerializeField] TextMeshProUGUI textNameServer;

    [Header("Прогресс заполняемости сервера")]
    [SerializeField] RadialProgress radialProgress;


    // Use this for initialization
    void Start()
        {
        Ini();
        }

    private void Ini ()
    {

        if (radialProgress == null)
        {
            throw new SlotServerException("radial progress not seted");
        }


        if (button == null)
        {
            if (!TryGetComponent(out button))
            {
                throw new SlotServerException($"{name} not have component UnityEngine.UI.Button");
            }

            button.onClick.AddListener(Select);
        }

        radialProgress.UpdateProgress(1f);

    }

    private void Select()
    {
        if (serverSlotData == null)
        {
            throw new SlotServerException($"{name} not have data server slot");
        }

        onClick?.Invoke(serverSlotData);
    }

    public void SetText(string text)
    {
        Ini();

        if (serverSlotData == null)
        {
            throw new SlotServerException($"{name} not have data server slot");
        }

        textNameServer.text = text;
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

        serverSlotData = data;

        CheckValidStats();


        SetText($"{transform.GetSiblingIndex() + 1}. {serverSlotData.NameServer}");


       
    }

    public void CheckValidStats()
    {
        foreach (var prop in serverSlotData.GetType().GetFields())
        {
            try
            {
                ValidatorData.CheckValidFieldStats(prop, serverSlotData);
            }
            catch (ValidatorDataException)
            {

                throw;
            }
        }

        foreach (var prop in serverSlotData.GetType().GetProperties())
        {
            try
            {
                ValidatorData.CheckValidPropertyStats(prop, serverSlotData);
            }
            catch (ValidatorDataException)
            {

                throw;
            }
        }
    }

}