using UnityEngine;

public class PointSpawn : MonoBehaviour
    {
    private const int RADUIS_GIZMOZ_SPHERE = 1;

    private Color _defaultColor = Color.green;

    public Vector3 Position => transform.position;


    private void OnDrawGizmos()
    {
        Gizmos.color = _defaultColor;

        Gizmos.DrawSphere(transform.position, RADUIS_GIZMOZ_SPHERE);
    }

}