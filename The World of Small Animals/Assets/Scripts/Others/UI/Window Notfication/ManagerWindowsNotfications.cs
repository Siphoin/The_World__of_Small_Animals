using UnityEngine;

public class ManagerWindowsNotfications : MonoBehaviour, IRemoveObject
    {
    private const string PATH_PREFAB_MESSAGE_NOTFICATION = "Prefabs/UI/windowNotfication";

    private const string PATH_PREFAB_MESSAGE_NOTFICATION_ERROR = "Prefabs/UI/windowNotficationError";

    private WindowNotfication windowNotficationMessagePrefab;

    private WindowNotfication windowNotficationErrorPrefab;

    private static WindowNotfication activeNotfication;

    private static ManagerWindowsNotfications manager;

    public static ManagerWindowsNotfications Manager { get => manager; }

    // Use this for initialization
    void Awake()
        {
        if (manager == null)
        {
            windowNotficationErrorPrefab = Resources.Load<WindowNotfication>(PATH_PREFAB_MESSAGE_NOTFICATION_ERROR);

            if (windowNotficationErrorPrefab == null)
            {
                throw new ManagerWindowsNotficationsException("window notfication error prefab not found");
            }

            windowNotficationMessagePrefab = Resources.Load<WindowNotfication>(PATH_PREFAB_MESSAGE_NOTFICATION);

            if (windowNotficationMessagePrefab == null)
            {
                throw new ManagerWindowsNotficationsException("window notfication message prefab not found");
            }

            DontDestroyOnLoad(gameObject);

            manager = this;
        }

        else
        {
            Remove();
        }
        }

    public void Remove()
    {
        Destroy(gameObject);
    }

    public void Remove(float time)
    {
        Destroy(gameObject, time);
    }

    public WindowNotfication CreateNotfication (string text, MessageNotficationType typeNotfication = MessageNotficationType.Message, bool loadStartScene = false)
    {

        if (!Application.isPlaying)
        {
            return null;
        }
        if (activeNotfication != null)
        {
            return activeNotfication;
        }

        WindowNotfication notfication = null;

        switch (typeNotfication)
        {
            case MessageNotficationType.Message:
                notfication = Instantiate(windowNotficationMessagePrefab);
                break;
            case MessageNotficationType.Error:
                notfication = Instantiate(windowNotficationErrorPrefab);
                break;
            default:
                throw new ManagerWindowsNotficationsException("message type invalid");
        }

        notfication.SetText(text);
        notfication.LoadStartScene = loadStartScene;

        activeNotfication = notfication;

        return notfication;
    }

}