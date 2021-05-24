using UnityEditor;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/UI/Smiles Container", order = 0)]
public class SmileContainer : ScriptableObject
    {
    [Header("Список доступных смайлов для отправки игроку")]
    [SerializeField] private Sprite[] smilesSprites;

    public Sprite[] SmilesSprites { get => smilesSprites; }
}