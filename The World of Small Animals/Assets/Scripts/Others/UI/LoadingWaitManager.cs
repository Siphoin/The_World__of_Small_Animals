using System.Collections;
using UnityEngine;

    public class LoadingWaitManager : MonoBehaviour, IRemoveObject
    {
    private const string PATH_PREFAB_WAIT_LOADING = "Prefabs/UI/loadingRequestWindow";
    
    private LoadingWait _loadingWaitPrefab;

    private static LoadingWaitManager _manager;

    public static LoadingWaitManager Manager { get => manager; }

   private void Awake()
        {
        if (_manager == null)
        {

            loadingWaitPrefab = Resources.Load<LoadingWait>(PATH_PREFAB_WAIT_LOADING);

            if (_loadingWaitPrefab == null)
            {
                throw new LoadingWaitException("loading wait prefab not found");
            }


            DontDestroyOnLoad(gameObject);
            _manager = this;
        }

        else
        {
            Remove();
        }
        }


    public  LoadingWait CreateLoadingWait ()
    {
        return Instantiate(_loadingWaitPrefab);
    }
    
    public void Remove() => Destroy(gameObject);

    public void Remove(float time) =>  Destroy(gameObject, time);
}
