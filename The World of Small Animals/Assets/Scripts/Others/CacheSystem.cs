using System;
using System.IO;
using UnityEngine;

public static class CacheSystem
    {
    private const string PATH_MAIN_FOLBER_CACHE = "local/";
    private static string GetAssetPath ()
    {
        string assetPath = Application.platform == RuntimePlatform.WindowsEditor ? Application.dataPath : Application.persistentDataPath;

        string path = assetPath + PATH_MAIN_FOLBER_CACHE;

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);

#if UNITY_EDITOR
            Debug.Log($"directory main folber has created: Path: {path}");
#endif
        }


        return path;
    }

    public static string ReadFile (string path)
    {
        path = GetAssetPath() + path;
        if (!File.Exists(path))
        {
            throw new CacheSystemException($"file on path {path} not found");
        }
        try
        {
        return File.ReadAllText(path);
        }
        catch (Exception e)
        {
            ShowErrorNotfication($"Произошла системная ошибка игры. Не удалось прочесть файл.\nОшибка: {e.Message}");
            return null;
        }

    }

    public static void WriteFile (string path, string text)
    {
        path = GetAssetPath() + path;

        try
        {
             File.WriteAllText(path, text);

#if UNITY_EDITOR
            Debug.Log($"File {path} is saved.");
#endif
        }
        catch (Exception e)
        {

            ShowErrorNotfication($"Произошла системная ошибка игры. Не удалось записать файл.\nОшибка: {e.Message}");
        }
    }

    public static void DeleteFile(string path)
    {
        path = GetAssetPath() + path;


        if (!File.Exists(path))
        {
            throw new CacheSystemException($"file on path {path} not found");
        }
        try
        {
             File.Delete(path);
        }
        catch (Exception e)
        {
            ShowErrorNotfication($"Произошла системная ошибка игры. Не удалось удалить файл.\nОшибка: {e.Message}");
        }

    }

    public static bool FileExits (string path)
    {
        path = GetAssetPath() + path;

        return File.Exists(path);
    }



   private static void ShowErrorNotfication (string text) => ManagerWindowsNotfications.Manager.CreateNotfication(text, MessageNotficationType.Error);
    }
