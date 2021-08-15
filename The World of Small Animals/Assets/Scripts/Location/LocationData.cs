using UnityEngine;

[CreateAssetMenu(menuName = "Data/Location/Location Data", order = 0)]
public class LocationData : ScriptableObject
    {
        [Header("Название локации")]
        [SerializeField]
        private string _nameLocation;

        [Header("Музыка на локации")]
        [SerializeField]
        private AudioClip _musicLocation;

    public string NameLocation => _nameLocation;
    public AudioClip MusicLocation => _musicLocation;
}