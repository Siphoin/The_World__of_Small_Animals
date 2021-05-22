using System.Collections;
using UnityEngine;

    public class ListenerTouchPlayer : MonoBehaviour
    {

    private const string PATH_PREFAB_TOUCH_EFFECT = "Prefabs/UI/circle_touch";
    private const string TAG_MY_PLAYER = "MyPlayer";



    [Header("Канвас, на котором будет появлться эффект")]
    [SerializeField] private Transform mainCanvas;

    private  CharacterController myPlayer;

    private GameObject touchEffectPrefab;
        // Use this for initialization
        void Awake()
        {

        if (mainCanvas == null)
        {
            throw new ListenerTouchPlayerException("mainCanvas not seted");
        }
        myPlayer = GameObject.FindGameObjectWithTag(TAG_MY_PLAYER).GetComponent<CharacterController>();


        touchEffectPrefab = Resources.Load<GameObject>(PATH_PREFAB_TOUCH_EFFECT);

        if (touchEffectPrefab == null)
        {
            throw new ListenerTouchPlayerException("touch effect prefab not found");
        }

        myPlayer.onMove += MyPlayer_onMove;
        }

    private void MyPlayer_onMove(Vector3 position)
    {
        GameObject touchEffect = Instantiate(touchEffectPrefab, mainCanvas);
       touchEffect.transform.position = Camera.main.WorldToScreenPoint(position);
    }

    private void OnDestroy()
    {
        try
        {
            if (myPlayer != null)
            {
                myPlayer.onMove -= MyPlayer_onMove;
            }
        }
        catch
        {

        }
    }
}