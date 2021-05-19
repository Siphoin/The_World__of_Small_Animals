using System.Collections;
using TMPro;
using UnityEngine;

    public class PanelNameLocation : MonoBehaviour
    {
    [Header("Текст имени локации")]
    [SerializeField] private TextMeshProUGUI textNameLocation;
        // Use this for initialization
        void Start()
    {
        Ini();

    }

    private void Ini()
    {
        if (textNameLocation == null)
        {
            throw new PanelNameLocationException("text name location not seted");
        }
    }

    public void SetText (string text)
    {
        Ini();
        textNameLocation.text = text;
    }

    }