using System.Collections;
using UnityEngine;

    public class ActivatorGameObject : MonoBehaviour, IActivatorGameObject
    {
        public void SetActiveSelfGameObject(GameObject gameObject, bool activeState)
        {
            gameObject.SetActive(activeState);
        }

    }