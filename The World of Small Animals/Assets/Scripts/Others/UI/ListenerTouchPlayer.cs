using System.Collections;
using UnityEngine;

    public class ListenerTouchPlayer : MonoBehaviour
    {

    private const string PATH_PREFAB_TOUCH_EFFECT = "Prefabs/UI/circle_touch";
    private const string TAG_MY_PLAYER = "MyPlayer";



    [Header("Канвас, на котором будет появлться эффект")]
    [SerializeField] private Transform _mainCanvas;

    private  CharacterController _myPlayer;

    private GameObject _touchEffectPrefab;
    
       private void Awake()
        {

        if (_mainCanvas == null)
        {
            throw new ListenerTouchPlayerException("mainCanvas not seted");
        }

        _myPlayer = GameObject.FindGameObjectWithTag(TAG_MY_PLAYER).GetComponent<CharacterController>();


        _touchEffectPrefab = Resources.Load<GameObject>(PATH_PREFAB_TOUCH_EFFECT);

        if (_touchEffectPrefab == null)
        {
            throw new ListenerTouchPlayerException("touch effect prefab not found");
        }

        _myPlayer.OnMove += MyPlayer_onMove;
        }

    private void MyPlayer_onMove(Vector3 position)
    {
        GameObject touchEffect = Instantiate(_touchEffectPrefab, _mainCanvas);

       touchEffect.transform.position = Camera.main.WorldToScreenPoint(position);
    }

    private void OnDestroy()
    {
        try
        {
            if (_myPlayer != null)
            {
                _myPlayer.OnMove -= MyPlayer_onMove;
            }
        }
        catch
        {

        }
    }
}
