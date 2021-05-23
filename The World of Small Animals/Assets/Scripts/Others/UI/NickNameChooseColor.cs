using UnityEngine;
[CreateAssetMenu(menuName = "Data/UI/NickName/NickName Choose Color", order = 0)]
public class NickNameChooseColor : ScriptableObject
    {
    [Header("Цвет никнейма при персонаже, который не принадлежит игроку")]
    [SerializeField] private Color defaultColorText = Color.white;

    [Header("Цвет никнейма при персонаже, который принадлежит игроку")]
    [SerializeField] private Color isMineColorText = Color.green;

    [Header("Цвет никнейма при персонаже, который является другом локальному игроку")]
    [SerializeField] private Color friendColorText = Color.green;

    public Color DefaultColorText { get => defaultColorText; }
    public Color IsMineColorText { get => isMineColorText; }
    public Color FriendColorText { get => friendColorText; }
}