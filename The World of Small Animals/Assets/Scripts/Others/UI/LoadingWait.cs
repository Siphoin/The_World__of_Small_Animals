using UnityEngine;
using TMPro;
using System;

public class LoadingWait : Window, ISeterText
    {
    [Header("Текст информации о запросе")]
    [SerializeField] private TextMeshProUGUI _textWait;

    private static LoadingWait _activeWait = null;

   
  private  void Start()
        {

        if (_textWait == null)
        {
            throw new LoadingWaitException("text wait not seted");
        }


        if (_activeWait != null)
        {
            Exit();
        }
        else
        {
        _activeWait = this;
        DontDestroyOnLoad(gameObject);
        }

        }

    public void SetText(string text) => _textWait.text = text;

}
