using UnityEngine;
[CreateAssetMenu(menuName = "Data/UI/Server Slot Color Data", order = 0)]
public class ServerSlotColorData : ScriptableObject
    {
    [Header("Цвет при переполненном сервере")]
    [SerializeField] Color blockedButtonColor;

    public Color BlockedButtonColor { get => blockedButtonColor; }
}