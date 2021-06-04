using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WindowNotfication : Window, ISeterText
    {

    private const string START_SCENE_NAME = "auth";

    public bool LoadStartScene { get; set; } = false;

    [Header("Текст сообщения")]
    [SerializeField] private TextMeshProUGUI textMessage;

    [Header("Кнопка закрытия окна")]
    [SerializeField] private Button buttonExit;

    [Header("Кнопка закрытия окна (2)")]
    [SerializeField] private Button buttonExitTwo;

    public void SetText(string text)
    {
        Ini();

        textMessage.text = text;
    }


    // Use this for initialization
    void Start()
        {
        Ini();

        buttonExit.onClick.AddListener(Exit);
        buttonExitTwo.onClick.AddListener(Exit);
    }

    public override void Exit()
    {
        if (LoadStartScene)
        {
            if (SceneManager.GetActiveScene().name != START_SCENE_NAME)
            {
            Loading.LoadScene(START_SCENE_NAME);
            }

        }


        base.Exit();
    }

    private void Ini ()
    {
        if (textMessage == null)
        {
            throw new WindowNotficationException("text message not seted");
        }

        if (buttonExit == null)
        {
            throw new WindowNotficationException("button exit not seted");
        }

        if (buttonExitTwo == null)
        {
            throw new WindowNotficationException("button exit two not seted");
        }
    }


}