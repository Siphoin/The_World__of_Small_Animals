using UnityEngine;
[CreateAssetMenu(menuName = "Data/Character/Character Camera Angle Settings", order = 0)]
public class CharacterCameraAngles : ScriptableObject
    {
    [Header("Список ракурсов")]
    [SerializeField]
    private Sprite[] cameraAngles = new Sprite[8];

    public Sprite[] CameraAngles { get => cameraAngles; }
}