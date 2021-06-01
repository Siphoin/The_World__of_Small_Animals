using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;

    public class AuthForm : AuthFormBase, ICallerLoadingWaitWindow
    {
    private const string TEXT_REQUEST_TOKEN_EXITS = "token already exits";


    // Use this for initialization
    void Start()
        {
        Ini();
        }

    public override void Ini()
    {

        base.Ini();


        webForm.onRequestFinish += ReceiveDataForm;
        webForm.onSubmit += WebForm_onSubmit;
        webForm.onInvalidData += WebForm_onInvalidData;
        authUser.onAuthFinish += AuthingUser;
    }

    private void AuthingUser()
    {
        SetTextActiveLoadingWait("Еще минуточку...");

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
                    ManagerWindowsNotfications.Manager.CreateNotfication("Вы уже зашли в игру в этом аккаунте с одним из персонажей.");
                    DestroyLoadingWaitWindow();
                    return;
                }

                 if (text == "user not found")
                {
                    ManagerWindowsNotfications.Manager.CreateNotfication("Пользователь не найден. Возможно ты вел(a) неправильные данные. Пожалуйста, проверь все поля.");
                    DestroyLoadingWaitWindow();
                    return;
                }

                if (text == "password not found")
                {
                    ManagerWindowsNotfications.Manager.CreateNotfication("Пароль неверный. Возможно ты вел(a) неправильные данные. Пожалуйста, проверь, правильный ли пароль.");
                    DestroyLoadingWaitWindow();
                    return;
                }
                tokenString = JsonConvert.DeserializeObject<TokenString>(text);
               authUser.Auth(tokenString);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                ManagerWindowsNotfications.Manager.CreateNotfication("Произошла системная ошибка игры.", MessageNotficationType.Error);
                DestroyLoadingWaitWindow();
            }
        }
    }


    private void OnDestroy()
    {
        try
        {
            if (authCharacter)
            {
                authUser.onAuthFinish -= AuthingUser;
            }

           
        }
        catch 
        {

        }
    }

    private void AuthingCharacter()
    {
        
    }
}