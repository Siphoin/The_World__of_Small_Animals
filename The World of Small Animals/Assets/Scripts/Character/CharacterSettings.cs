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

    [Header("Через какое время игрок будет отключен от сервекра если персонаж не движется")]
    [SerializeField]
    private float afkTimeOut = 180;

    public float Speed { get => speed; }
    public float SpeedDecrement { get => speedDecrement; }
    public float AfkTimeOut { get => afkTimeOut; }
}