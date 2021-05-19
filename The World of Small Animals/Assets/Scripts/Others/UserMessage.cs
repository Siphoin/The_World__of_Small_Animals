using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[RequireComponent(typeof(ContentSizeFitter))]
[RequireComponent(typeof(TextMeshProUGUI))]
public class UserMessage : MonoBehaviour
    {

    private const string PATH_USER_PARAMS_SETTINGS = "Params/UserMessageParams";
    private TextMeshProUGUI textData;
    private ContentSizeFitter contentSizeFitter;

    private UserMessageParams messageParams;
        // Use this for initialization
        void Start()
    {
        Ini();
    }

    private void Ini()
    {
        if (!TryGetComponent(out textData))
        {
            throw new UserMessageException($"{name} not have component TMPro.TextMeshProUGUI");
        }

        if (!TryGetComponent(out contentSizeFitter))
        {
            throw new UserMessageException($"{name} not have component Content Size Fitler");
        }
    }

    private void SetText (string text)
    {
        Ini();
        textData.text = text;
    }

    private void CheckFitlerSizeWidth ()
    {
        if (textData.text.Length >= messageParams.LengthCheckWidthMessage)
        {

        }
    }
}