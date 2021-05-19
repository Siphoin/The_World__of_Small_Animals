using UnityEngine;

[CreateAssetMenu(menuName = "Data/Location/Location Data", order = 0)]
public class LocationData : ScriptableObject
    {
        [Header("Название локации")]
        [SerializeField]
        private string nameLocation;

        [Header("Музыка на локации")]
        [SerializeField]
        private AudioClip musicLocation;

    public string NameLocation { get => nameLocation; }
    public AudioClip MusicLocation { get => musicLocation; }
}