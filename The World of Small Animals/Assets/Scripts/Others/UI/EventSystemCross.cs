using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemCross : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
#if UNITY_EDITOR
        gameObject.AddComponent<StandaloneInputModule>().forceModuleActive = true;
#endif



#if UNITY_ANDROID
        gameObject.AddComponent<TouchInputModule>().forceModuleActive = true;
#endif
    }

    // Update is called once per frame
    void Update()
        {

        }
    }