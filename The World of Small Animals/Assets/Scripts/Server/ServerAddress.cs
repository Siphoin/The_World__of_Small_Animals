using UnityEngine;
[CreateAssetMenu(menuName = "Server/Server Address", order = 0)]
public class ServerAddress : ScriptableObject
    {
    [Header("Адрес сервера БД")]
    [TextArea]
    [SerializeField] private string address = "";

    public string Address { get => address; }
}