using UnityEngine;

public interface IInstantiateNetworkObject
    {
    GameObject InstantiatePlayerObject(GameObject gameObject, Vector3 position, Quaternion rotation, byte group = 0, object[] data = null);
    GameObject InstantiateSceneObject(GameObject gameObject, Vector3 position, Quaternion rotation, byte group = 0, object[] data = null);

    GameObject InstantiatePlayerObject(GameObject gameObject, Vector3 position, byte group = 0, object[] data = null);
    GameObject InstantiateSceneObject(GameObject gameObject, Vector3 position, byte group = 0, object[] data = null);
}
