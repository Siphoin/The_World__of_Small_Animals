using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
    {
    private static string sceneName = "servers";


    private const string SCENE_NAME_LOADING = "loading";


    private const string PATH_PREFAB_BACKGROUND_LOADING_FINISH = "Prefabs/UI/LoadingFinishAnimation";

    private static GameObject _backgroundFinishLoad;

    public static string LastSceneName { get; private set; }

        void Start()
        {
        Ini();

        LoadSceneAsync();

        }

    private IEnumerator LoadSceneProgress ()
    {
        yield return new WaitForSecondsRealtime(4);
        AsyncOperation  asyncOperation = SceneManager.LoadSceneAsync(sceneName);


        while (!asyncOperation.isDone)
        {

            float progress = asyncOperation.progress / 0.9f;

            if (progress >= 0.9f)
            {
                GameObject backgroundFinish = Instantiate(_backgroundFinishLoad);
                DontDestroyOnLoad(backgroundFinish);


            }
            yield return null;
        }
    }

    private void LoadSceneAsync ()
    {
        StartCoroutine(LoadSceneProgress());
    }

    public static void LoadScene (string nameScene)
    {
        sceneName = nameScene;


        try
        {
            LastSceneName = SceneManager.GetActiveScene().name;

            SceneManager.LoadScene(SCENE_NAME_LOADING);
        }
        catch 
        {

            throw new LoadingException($"scene {SCENE_NAME_LOADING} not exits in Build Settings");
        }
    }


    private static void Ini ()
    {
        if (_backgroundFinishLoad != null)
        {
            return;
        }


        _backgroundFinishLoad = Resources.Load<GameObject>(PATH_PREFAB_BACKGROUND_LOADING_FINISH);

        if (_backgroundFinishLoad == null)
        {
            throw new LoadingException("prefab background loading finish not found");
        }
    }
    }