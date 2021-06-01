using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


    public class CheckboxWebForm : WebFormFragment, IWebFormFragment
    {
    private Toggle toggle;
    // Use this for initialization
    void Start()
        {
        Ini();
        }

        // Update is called once per frame

    public object GetValue()
    {
        return Convert.ToInt32(toggle.isOn);
    }

    public override void Ini()
    {
        base.Ini();

       if (!TryGetComponent(out toggle))
        {
            throw new CheckboxWebFormException($"{name} not have component Toggle");
        }
    }
}