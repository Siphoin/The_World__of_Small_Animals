using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SmileWindowOpenButton : ActivatorGameObject
    {
    [Header("Окно для активации")]
    [SerializeField] GameObject windowTarget;

    private Button button;
        // Use this for initialization
        void Start()
        {
        if (windowTarget == null)
        {
            throw new SmileWindowOpenButtonException("window target not seted");
        }

        if (!TryGetComponent(out button))
        {
            throw new SmileWindowOpenButtonException($"{name} not have component UnityEngine.UI.Button");
        }

        button.onClick.AddListener(SetStateActiveWindow);
        }

    public void SetStateActiveWindow()
    {
        bool state = !windowTarget.activeSelf;

        SetActiveSelfGameObject(windowTarget, state);
    }
}