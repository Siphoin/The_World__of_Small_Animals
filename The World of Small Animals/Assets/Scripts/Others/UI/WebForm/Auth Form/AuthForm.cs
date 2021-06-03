﻿using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;

    public class AuthForm : AuthFormBase, ICallerLoadingWaitWindow
    {
    private const string TEXT_REQUEST_TOKEN_EXITS = "token already exits";

    private const string SCENE_NAME_SELECT_CHARACTER = "characters";


    // Use this for initialization
    void Start()
        {
        Ini();
        }

    public override void Ini()
    {

        base.Ini();


        authUser = AuthUser.Manager == null ? authUser : AuthUser.Manager;


        webForm.onRequestFinish += ReceiveDataForm;
        webForm.onSubmit += WebForm_onSubmit;
        webForm.onInvalidData += WebForm_onInvalidData;
        authUser.onAuthFinish += AuthingUser;
        authUser.onAuthFalled += AuthingUserFalled;
    }

    private void AuthingUserFalled()
    {
        ShowNotficationInvalidData("Не удалось войти в учетную запись, попробуйте зайти позже.");
    }

    private void AuthingUser()
    {
        UncribeAuthEvents();
        DestroyLoadingWaitWindow();
        Loading.LoadScene(SCENE_NAME_SELECT_CHARACTER);

    }

    private void UncribeAuthEvents()
    {
        authUser.onAuthFinish -= AuthingUser;
        authUser.onAuthFalled -= AuthingUserFalled;
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
                ManagerWindowsNotfications.Manager.CreateNotfication("Произошла системная ошибка игры.", MessageNotficationType.Error);
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
}