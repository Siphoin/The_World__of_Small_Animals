using UnityEngine;

public class InitilizatorLocation : MonoBehaviour, IRemoveObject
    {
    private const string PATH_PREFAB_JOIN_LOCATION_OBJECT = "System/JoinLocation";


    private JoinLocation _joinLocationPrefab;

    [Header("Данные о локации")]
    [SerializeField]
    private LocationData _locationData;

    void Start()
        {
        if (_locationData == null)
        {
            throw new InitilizatorLocationException("location data not seted");
        }

        if (string.IsNullOrEmpty(_locationData.NameLocation))
        {
            throw new InitilizatorLocationException("name location is null");
        }



        _joinLocationPrefab = Resources.Load<JoinLocation>(PATH_PREFAB_JOIN_LOCATION_OBJECT);

        if (_joinLocationPrefab == null)
        {
            throw new InitilizatorLocationException("join location prefab not found");
        }

        Instantiate(_joinLocationPrefab).SetLocationData(_locationData);

        Remove();

        }
    public void Remove() => Destroy(gameObject);

    public void Remove(float time) => Destroy(gameObject, time);
}