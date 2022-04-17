using TMPro;
using UnityEngine;

public class PanelNameLocation : MonoBehaviour
    {
    [Header("Текст имени локации")]
    [SerializeField] private TextMeshProUGUI _textNameLocation;

    private void Ini()
    {
        if (_textNameLocation == null)
        {
            throw new PanelNameLocationException("text name location not seted");
        }
    }

    public void SetText (string text)
    {
        Ini();

        _textNameLocation.text = text;
    }

   private void Start() => Ini();

}
