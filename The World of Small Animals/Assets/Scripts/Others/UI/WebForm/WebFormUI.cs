using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

public class WebFormUI : MonoBehaviour, IRequestSender
    {
    [Header("Поля данных")]
    [SerializeField] WebFormFragment[] fragments;
    [Header("Страница API для отправки")]
    [SerializeField] private string urlSend;

    [Header("Валидировать данные перед отправкой?")]
    [SerializeField] private bool validingData;

    [Header("Submit кнопка для отправки (не обязательно)")]
    [SerializeField] private Button submitButton;

    private string idRequest;

    private Dictionary<string, object> form = new Dictionary<string, object>();

    public event Action onSubmit;

    public event Action<WebFormTypeInvalidData> onInvalidData;


    public event Action<string, RequestResult, long> onRequestFinish;

    private Dictionary<string, WebFormFragment> requiredFragments = new Dictionary<string, WebFormFragment>();
        // Use this for initialization
        void Start()
        {
        Ini();
        }


    private void Ini ()
    {

        if (string.IsNullOrEmpty(urlSend))
        {
            throw new WebFormException($"web form {name} any null url send");
        }


        if (fragments.Any(f => f == null))
        {
            throw new WebFormException($"web form {name} any null fragments");
        }

        if (submitButton)
        {
            submitButton.onClick.AddListener(Submit);
        }

        for (int i = 0; i < fragments.Length; i++)
        {
            WebFormFragment fragment = fragments[i];

            requiredFragments.Add(fragment.Name, fragment);
        }

#if UNITY_EDITOR
        Debug.Log($"web form {name} requered {requiredFragments.Count} web fragments");
#endif
    }

    public void Submit ()
    {
        if (!DataisValid() && validingData)
        {
            return;
        }
        form.Clear();


        foreach (var fragment in requiredFragments)
        {
            string key = fragment.Key;
            object value = null;

            IWebFormFragment iInterfaceWebFragment = (IWebFormFragment)fragment.Value;

            value = iInterfaceWebFragment.GetValue();

            bool isString = value.IsString();

            if (isString)
            {
                form.Add(key, (string)value);
            }

            else
            {
                form.Add(key, (int)value);
            }
        }

        onSubmit?.Invoke();


        SendRequest();

        
    }

    public void SendRequest()
    {
        RequestManager requestManager = RequestManager.Manager;
        requestManager.onRequestFinish += ReceiveRequest;

        idRequest = requestManager.GenerateRequestID();

        requestManager.SendRequestToServer(idRequest, urlSend, RequestType.POST, form);
    }

    public void ReceiveRequest(string id, string text, RequestResult requestResult, long responseCode)
    {
        if (id == idRequest)
        {
        onRequestFinish?.Invoke(text, requestResult, responseCode);
        RequestManager.Manager.onRequestFinish -= ReceiveRequest;
        }

    }

    private void SendEventInvalidData (WebFormTypeInvalidData type)
    {
        onInvalidData?.Invoke(type);
    }

    public bool DataisValid()
    {
        foreach (var fragment in requiredFragments)
        {

            object value = null;

            IWebFormFragment iInterfaceWebFragment = (IWebFormFragment)fragment.Value;

            value = iInterfaceWebFragment.GetValue();

            bool isString = value.IsString();

            if (isString)
            {
                string str = (string)value;

                if (string.IsNullOrEmpty(str))
                {
                    SendEventInvalidData(WebFormTypeInvalidData.Input);
                    return false;
                }
            }

            else
            {

                if (iInterfaceWebFragment is CheckboxWebForm)
                {
                    bool checkValue = Convert.ToBoolean((int)value);
                    if (!checkValue)
                    {
                        SendEventInvalidData(WebFormTypeInvalidData.Checkbox);
                        return false;
                    }
                }
            }
        }
        return true;
    }
}