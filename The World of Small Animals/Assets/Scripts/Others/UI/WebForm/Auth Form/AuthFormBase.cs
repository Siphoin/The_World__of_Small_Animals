using System.Collections;
using UnityEngine;

    public class AuthFormBase : MonoBehaviour, ICallerLoadingWaitWindow
    {
    protected LoadingWait loadingWaitActive;

    [Header("Форма для авторизации пользователя")]
    [SerializeField] protected WebFormUI webForm;
    [Header("Менеджер аутификации персонажа")]
    [SerializeField] protected AuthCharacter authCharacter;

    [Header("Менеджер аутификации пользователя")]
    [SerializeField] protected AuthUser authUser;

    protected TokenString tokenString;
    // Use this for initialization
    void Start()
        {

        }


    public void DestroyLoadingWaitWindow()
    {
        loadingWaitActive.Remove();
    }

    public void CreateLoadingWaitWindow(string text = "Подождите...")
    {
        loadingWaitActive = LoadingWaitManager.Manager.CreateLoadingWait();
        SetTextActiveLoadingWait(text);
    }

    public void SetTextActiveLoadingWait (string text)
    {
        if (!loadingWaitActive)
        {
            throw new AuthFormBaseException("active loading wait is null");
        }

        loadingWaitActive.SetText(text);
    }

    public virtual void Ini ()
    {
        if (!webForm)
        {
            throw new AuthFormBaseException("web form not seted");
        }

        if (!authUser)
        {
            throw new AuthFormBaseException("auth user manager not seted");
        }

        if (!authUser)
        {
            throw new AuthFormBaseException("auth character manager not seted");
        }
    }
}