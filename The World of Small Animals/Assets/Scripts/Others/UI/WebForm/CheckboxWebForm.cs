using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


    public class CheckboxWebForm : WebFormFragment, IWebFormFragment
    {
    private Toggle toggle;
   

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

    public void SetValue(object value)
    {
        if (value is bool == false)
        {
            throw new CheckboxWebFormException("value not be bool");
        }

        toggle.isOn = (bool)value;
    }
    
    private void Start() => Ini();
}
