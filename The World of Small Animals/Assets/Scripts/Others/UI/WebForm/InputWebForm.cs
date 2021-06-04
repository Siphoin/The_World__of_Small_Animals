using System.Collections;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(TMP_InputField))]
    public class InputWebForm : WebFormFragment, IWebFormFragment
    {
    private TMP_InputField inputField;


    [Header("Тип поля")]
    [SerializeField] private InputWebFormType typeInput = InputWebFormType.Text;


    // Use this for initialization
    void Start()
        {
        Ini();
        }

    public object GetValue()
    {
        object val = null;


        string text = inputField.text.Trim();


        int number = 0;


        if (typeInput == InputWebFormType.Number)
        {
        if (!int.TryParse(text, out number))
        {
                throw new InputWebFormException($"{name} have invalid int32 value. type input seted as {typeInput}");
        }

        else
        {
            val = number;
        }
        }

        else
        {
            val = text;
        }

        return val;
    }

    public override void Ini()
    {
        base.Ini();

        if (inputField == null)
        {
        if (!TryGetComponent(out inputField))
        {
            throw new InputWebFormException($"{name} not have component TMP_InputField");
        }

        inputField.onEndEdit.AddListener(TrimText);
        }

    }

    private void TrimText(string arg0)
    {
        inputField.text = arg0.Trim();
    }

    public void SetValue(object value)
    {

        Ini();
        if (!value.IsString())
        {
            throw new InputWebFormException("value argument not be string");
        }

        inputField.text = (string)value;
    }
}