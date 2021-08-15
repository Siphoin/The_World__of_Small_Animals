using UnityEditor;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Character/Character Settings", order = 0)]
public class CharacterSettings : ScriptableObject
    {
    [Header("Скорость персонажа")]
    [SerializeField]
    private float _speed = 20;

    [Header("Скорость замедления персонажа")]
    [SerializeField]
    private float _speedDecrement = 0.5f;

    [Header("Через какое время игрок будет отключен от сервекра если персонаж не движется")]
    [SerializeField]
    private float _afkTimeOut = 180;

    public float Speed => _speed;
    public float SpeedDecrement => _speedDecrement; 
    public float AfkTimeOut => _afkTimeOut;
}