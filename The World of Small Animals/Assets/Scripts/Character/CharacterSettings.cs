using UnityEditor;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Character/Character Settings", order = 0)]
public class CharacterSettings : ScriptableObject
    {
    [Header("Скорость персонажа")]
    [SerializeField]
    private float speed = 20;

    [Header("Скорость замедления персонажа")]
    [SerializeField]
    private float speedDecrement = 0.5f;

    public float Speed { get => speed; }
    public float SpeedDecrement { get => speedDecrement; }
}