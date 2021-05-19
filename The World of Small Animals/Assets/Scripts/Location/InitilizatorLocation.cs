using System.Collections;
using UnityEngine;

    public class InitilizatorLocation : MonoBehaviour, IRemoveObject
    {

    private const string PATH_PREFAB_PANEL_NAME_LOCATION = "Prefabs/UI/locationPanel";

    private const string NAME_CANVAS_MAIN = "MainCanvas";

    private PanelNameLocation panelNameLocationPrefab;

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


        panelNameLocationPrefab = Resources.Load<PanelNameLocation>(PATH_PREFAB_PANEL_NAME_LOCATION);

        if (panelNameLocationPrefab == null)
        {
            throw new InitilizatorLocationException("panel location name prefab not found");
        }

        GameObject mainCanvas = GameObject.Find(NAME_CANVAS_MAIN);

        Instantiate(panelNameLocationPrefab, mainCanvas.transform).SetText(locationData.NameLocation);

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