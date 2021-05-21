using System.Collections;
using UnityEngine;

    public class InitilizatorLocation : MonoBehaviour, IRemoveObject
    {


    private const string PATH_PREFAB_JOIN_LOCATION_OBJECT = "System/JoinLocation";


    private JoinLocation joinLocationPrefab;

    [Header("Данные о локации")]
    [SerializeField]
    private LocationData locationData;



    // Use this for initialization
    void Start()
        {
        if (locationData == null)
        {
            throw new InitilizatorLocationException("location data not seted");
        }

        if (string.IsNullOrEmpty(locationData.NameLocation))
        {
            throw new InitilizatorLocationException("name location is null");
        }



        joinLocationPrefab = Resources.Load<JoinLocation>(PATH_PREFAB_JOIN_LOCATION_OBJECT);

        if (joinLocationPrefab == null)
        {
            throw new InitilizatorLocationException("join location prefab not found");
        }



        Instantiate(joinLocationPrefab).SetLocationData(locationData);

        Remove();

        }
    public void Remove()
    {
        Destroy(gameObject);
    }

    public void Remove(float time)
    {
        Destroy(gameObject, time);
    }
}