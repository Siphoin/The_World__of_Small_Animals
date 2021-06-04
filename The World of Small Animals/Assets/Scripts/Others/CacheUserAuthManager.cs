using Newtonsoft.Json;
using UnityEngine;

public class CacheUserAuthManager : MonoBehaviour, IShowErrorNotfication
    {
    private const string NAME_FILE_AUTH_FILE = "auth.json";


    public AuthUserCache CacheData { get; private set; }

    public bool FileExits { get; private set; }


    // Use this for initialization
    void Awake()
        {
        FileExits = CacheSystem.FileExits(NAME_FILE_AUTH_FILE);

        if (FileExits)
        {
            try
            {
                CacheData = JsonConvert.DeserializeObject<AuthUserCache>(CacheSystem.ReadFile(NAME_FILE_AUTH_FILE));
            }
            catch (CacheUserAuthManagerException e)
            {
                ShowErrorNotfication($"Произошла системная ошибка игры. Не удалось прочесть файл авторизации.\nОшибка: {e.Message}");
                
            }
        }


        }

    public void SaveAuthData (string name, string password)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new CacheUserAuthManagerException("name argument is null");
        }

        if (string.IsNullOrEmpty(password))
        {
            throw new CacheUserAuthManagerException("password argument is null");
        }

        password = StringCipher.Encrypt(password);
        name = StringCipher.Encrypt(name);

        AuthUserCache cache = new AuthUserCache(name, password);
        string json = JsonConvert.SerializeObject(cache);
        CacheSystem.WriteFile(NAME_FILE_AUTH_FILE, json);

#if UNITY_EDITOR
        Debug.Log($"file auth local user saved. JSON:\n{json}");
#endif
    }

    public void DeleteAuthData()
    {
        if (FileExits)
        {
            CacheSystem.DeleteFile(NAME_FILE_AUTH_FILE);
            FileExits = false;
        }

        else
        {
#if UNITY_EDITOR
            Debug.Log($"file auth data local user not found");
#endif
        }
    }

    public void ShowErrorNotfication(string text)
    {
        ManagerWindowsNotfications.Manager.CreateNotfication(text, MessageNotficationType.Error);
    }



}