using System.Collections;
using UnityEngine;

    public class LoadingWaitManager : MonoBehaviour, IRemoveObject
    {
    private const string PATH_PREFAB_WAIT_LOADING = "Prefabs/UI/loadingRequestWindow";
    private LoadingWait loadingWaitPrefab;

    private static LoadingWaitManager manager;

    public static LoadingWaitManager Manager { get => manager; }

    // Use this for initialization
    void Awake()
        {
        if (manager == null)
        {

            loadingWaitPrefab = Resources.Load<LoadingWait>(PATH_PREFAB_WAIT_LOADING);

            if (loadingWaitPrefab == null)
            {
                throw new LoadingWaitException("loading wait prefab not found");
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

    public  LoadingWait CreateLoadingWait ()
    {
        return Instantiate(loadingWaitPrefab);
    }
}