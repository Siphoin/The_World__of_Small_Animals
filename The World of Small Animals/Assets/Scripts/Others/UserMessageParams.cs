using UnityEngine;
[CreateAssetMenu(menuName = "Params/Message Params", order = 0)]
public class UserMessageParams : ScriptableObject
    {
    [Header("Расширять текст при значении длинны сообщения")]
    [SerializeField]
    private int lengthCheckWidthMessage = 15;

    public int LengthCheckWidthMessage { get => lengthCheckWidthMessage; }
}