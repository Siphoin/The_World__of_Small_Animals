using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AuthForm : AuthFormBase, ICallerLoadingWaitWindow, IShowErrorNotfication
    {
    private const string TEXT_REQUEST_TOKEN_EXITS = "token already exits";

    private const string SCENE_NAME_SELECT_CHARACTER = "characters";

    private const string NAME_KEY_FIELD_NAME = "name";

    private const string NAME_KEY_FIELD_PASSWORD = "password";

    [Header("Галочка запомнить меня")]
    [SerializeField] private Toggle checkboxSaveMe;


    // Use this for initialization
    void Start()
        {
        Ini();
        }

    public override void Ini()
    {
        if (!checkboxSaveMe)
        {
            throw new AuthFormException("checkbox save me not seted");
        }


        base.Ini();


        authUser = AuthUser.Manager == null ? authUser : AuthUser.Manager;


        webForm.onRequestFinish += ReceiveDataForm;
        webForm.onRequestFalled += DestroyLoadingWaitWindow;

        webForm.onSubmit += WebForm_onSubmit;
        webForm.onInvalidData += WebForm_onInvalidData;

        authUser.OnAuthFinish += AuthingUser;
        authUser.OnAuthFalled += AuthingUserFalled;


        checkboxSaveMe.isOn = cacheUserAuthManager.FileExits;
        if (cacheUserAuthManager.FileExits)
        {
            AuthUserCache cache = cacheUserAuthManager.CacheData;

            string password = StringCipher.Decrypt(cache.password);

            string name = StringCipher.Decrypt(cache.name);

            webForm.SetValueOfFragment(NAME_KEY_FIELD_PASSWORD, password);
            webForm.SetValueOfFragment(NAME_KEY_FIELD_NAME, name);
        }
    }

    private void AuthingUserFalled()
    {
        ShowNotficationInvalidData("Не удалось войти в учетную запись, попробуйте зайти позже.");
    }

    private void AuthingUser()
    {

        if (checkboxSaveMe.isOn)
        {
            cacheUserAuthManager.SaveAuthData((string)webForm.GetValueOfFragment(NAME_KEY_FIELD_NAME), (string)webForm.GetValueOfFragment(NAME_KEY_FIELD_PASSWORD));
        }

        else
        {
            if (cacheUserAuthManager.FileExits)
            {
                cacheUserAuthManager.DeleteAuthData();
            }
        }


        UncribeAuthEvents();
        DestroyLoadingWaitWindow();
        Loading.LoadScene(SCENE_NAME_SELECT_CHARACTER);

    }

    private void UncribeAuthEvents()
    {
        authUser.OnAuthFinish -= AuthingUser;
        authUser.OnAuthFalled -= AuthingUserFalled;
    }

    private void WebForm_onInvalidData(WebFormTypeInvalidData type)
    {
        string message = type == WebFormTypeInvalidData.Input ? "Пожалуйста, заполни все поля в форме входа." : "Не все галочки в кружочках установлены.";

        ManagerWindowsNotfications.Manager.CreateNotfication(message);
    }

    private void WebForm_onSubmit()
    {
        CreateLoadingWaitWindow("Авторизируемся...");
    }


    private void ReceiveDataForm(string text, RequestResult result, long responseCode)
    {
        if (result == RequestResult.OK && responseCode == 200)
        {
            try
            {


                 if (text == TEXT_REQUEST_TOKEN_EXITS)
                {
                    ShowNotficationInvalidData("Вы уже зашли в игру в этом аккаунте с одним из персонажей.");
                    return;
                }

                 if (text == "user not found")
                {
                    ShowNotficationInvalidData("Пользователь не найден. Возможно ты вел(a) неправильные данные. Пожалуйста, проверь все поля.");
                    return;
                }

                if (text == "password not found")
                {
                    ShowNotficationInvalidData("Пароль неверный. Возможно ты вел(a) неправильные данные. Пожалуйста, проверь, правильный ли пароль.");
                    return;
                }


                tokenString = JsonConvert.DeserializeObject<TokenString>(text);

               authUser.Auth(tokenString);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                ShowErrorNotfication("Произошла системная ошибка игры.");
                DestroyLoadingWaitWindow();
            }
        }
    }


    private void OnDestroy()
    {
        try
        {
            if (authUser)
            {
                UncribeAuthEvents();
            }

           
        }
        catch 
        {

        }
    }


    private void ShowNotficationInvalidData (string text)
    {
        ManagerWindowsNotfications.Manager.CreateNotfication(text);
        DestroyLoadingWaitWindow();
    }

    public void ShowErrorNotfication(string text)
    {
        ManagerWindowsNotfications.Manager.CreateNotfication(text, MessageNotficationType.Error);
    }
}