using UnityEngine;

public class DestroyedObject : MonoBehaviour, IRemoveObject
    {
        public void Remove() => Destroy(gameObject);

        public void Remove(float time) => Destroy(gameObject, time);

}