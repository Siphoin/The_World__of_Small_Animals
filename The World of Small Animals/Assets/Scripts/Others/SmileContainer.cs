using UnityEngine;

[CreateAssetMenu(menuName = "Data/UI/Smiles Container", order = 0)]
public class SmileContainer : ScriptableObject
    {
    [Header("Список доступных смайлов для отправки игроку")]
    [SerializeField] private Sprite[] _smilesSprites;

    public Sprite[] SmilesSprites => _smilesSprites;
}