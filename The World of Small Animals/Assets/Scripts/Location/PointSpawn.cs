using UnityEngine;

public class PointSpawn : MonoBehaviour
    {
    private Color defaultColor = Color.green;

    public Vector3 Position { get => transform.position; }


    private void OnDrawGizmos()
    {
        Gizmos.color = defaultColor;
        Gizmos.DrawSphere(transform.position, 1);
    }

}