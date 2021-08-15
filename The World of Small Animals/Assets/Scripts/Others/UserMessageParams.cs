using UnityEngine;

[CreateAssetMenu(menuName = "Params/Message Params", order = 0)]
public class UserMessageParams : ScriptableObject
    {
    [Header("Максимпльное кол-во символов в поле чата")]
    [SerializeField] private int _maxCountSymbolsInput = 30;

    [Header("Время уничтожения пользовательского сообщения")]
    [SerializeField] private float _timeDestroyUserMessage = 11;

    public int MaxCountSymbolsInput  => _maxCountSymbolsInput;
    public float TimeDestroyUserMessage =>  _timeDestroyUserMessage;
}