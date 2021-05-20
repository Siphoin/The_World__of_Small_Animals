using UnityEngine;
using TMPro;
using System;

public class LoadingWait : Window, ISeterText
    {
    [Header("Текст информации о запросе")]
    [SerializeField] private TextMeshProUGUI textWait;

    private static LoadingWait activeWait = null;

    // Use this for initialization
    void Start()
        {

        if (textWait == null)
        {
            throw new LoadingWaitException("text wait not seted");
        }


        if (activeWait != null)
        {
            Exit();
        }
        else
        {
        activeWait = this;
        DontDestroyOnLoad(gameObject);
        }

        }

    public void SetText(string text)
    {
        textWait.text = text;
    }

}