using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class CharacterController : MonoBehaviour, ISeterSprite, IPunObservable, IRPCSender, IInvokerMono
{
    private const string PATH_SETTINGS_CHARACTER = "Data/Character/CharacterSettings";
    private const string TAG_WALL_BLOCK = "WallBlock";
    private const string TAG_PLAYER = "Player";

    private bool _isInitialized = false;

    [Header("Текущая скорость персонажа")]
    [ReadOnlyField]
    [SerializeField]
    private float _currentSpeed = 0f;
    [Header("Скорость персонажа при старте")]
    [ReadOnlyField]
    [SerializeField]
    private float _startedSpeed = 0f;

    private Vector2 _positionMove;

    [Header("Настройки ракурсов")]
    [SerializeField]
    private CharacterCameraAngles _characterCameraAnglesSettings;

    private Rigidbody2D _body2d;


    [Header("Движется")]
    [ReadOnlyField]
    [SerializeField]
    private bool _move = false;

    [Header("Номер активного ракурса")]
    [ReadOnlyField]
    [SerializeField]
    private int _numberCameraAngle = 0;


    [Header("Активен")]
    [ReadOnlyField]
    [SerializeField]
    private bool _isActiveMove = true;

    private CharacterSettings _characterSettings;

    private PhotonView _photonView;

    private SpriteRenderer _spriteRenderer;

    public event Action<Vector3> OnMove;

    public event Action<bool> OnMoveStatusChanged;

    // rotation

    private List<TupleAngle> _cameraAngles =


        new List<TupleAngle> {


        new TupleAngle(-22.5f, 22.5f), 
        new TupleAngle(22.5f, 67.5f), 
        new TupleAngle(67.5f, 112.5f),
        new TupleAngle(112.5f, 167.5f), 
        new TupleAngle(-67.5f, -22.5f), 
        new TupleAngle(-112.5f, -67.5f), 
        new TupleAngle(-167.5f, -112.5f),
        new TupleAngle(float.MinValue, float.MaxValue) 

    };


    public bool ActiveMove => _isActiveMove;
    public float CurrentSpeed  => _currentSpeed;
    public bool Moved => _move; 
    public CharacterCameraAngles CharacterCameraAnglesSettings => _characterCameraAnglesSettings;


    private void Start()
    {
        Ini();

        _startedSpeed = _characterSettings.Speed;

        _currentSpeed = _startedSpeed;

    }

    private void Awake()
    {
        if (!TryGetComponent(out _spriteRenderer))
        {
            throw new CharacterException($"{name} not have component Sprite Renderer");
        }

        if (!TryGetComponent(out _photonView))
        {
            throw new CharacterException($"{name} not have component Photon View");
        }

        name = _photonView.Owner.NickName;

        tag = _photonView.IsMine ? $"My{TAG_PLAYER}" : TAG_PLAYER;

       
    }

    private void Ini()
    {
        if (_isInitialized)
        {
            return;
        }


        if (_characterCameraAnglesSettings == null)
        {
            throw new CharacterException("character camera angles not seted");
        }


        if (!TryGetComponent(out _body2d))
        {
            throw new CharacterException($"{name} not have component Rightbody2D");
        }

        _characterSettings = Resources.Load<CharacterSettings>(PATH_SETTINGS_CHARACTER);

        if (_characterSettings == null)
        {
            throw new CharacterException("character settings not found");
        }

        _isInitialized = true;
    }

    private void Control ()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0) == true && _isActiveMove)
        {
            if (!_move)
            {
                Vector2 mousePos = Input.mousePosition;
                SetWorldPositionMove(mousePos);
                StartMoving();
            }

        }
#endif

#if UNITY_ANDROID


        RaycastHit hit;

        if (Input.touchCount == 1 && activeMove)
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
            Vector3 touchPos = touch.position;
            SetWorldPositionMove(touchPos);
            StartMoving();
            }

        }

#endif

        CheckMove();
        CheckRotation();
    }

  private  void Update()
    {
        if (_photonView.IsMine)
        {
        	_body2d.velocity = Vector2.zero;
            Control();
        }

    }

   private void FixedUpdate () {

        if (_photonView.IsMine)
        {
        	_body2d.velocity = Vector2.zero;
        }
    }

    

    private void CheckMove()
    {

        if (!_isActiveMove)
        {
            return;
        }

        if (_move)
        {
            Vector3 targetPosMove = Vector2.MoveTowards(transform.position, _positionMove, _currentSpeed * Time.deltaTime);

            _body2d.MovePosition(targetPosMove);
            

            if (Vector2.Distance(transform.position, _positionMove) <= 0.01f)
            {
                SetActiveCharacter(false);
               
            }
        }
    }

    private void CheckRotation ()
    {
        if (!_isActiveMove)
        {
            return;
        }

#if UNITY_EDITOR
        Vector3 posMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RootCameraAngleCharacter(posMouse);
#endif



#if UNITY_ANDROID


        if (Input.touchCount == 1)
        {
            Touch touch = Input.touches[0];
            Vector3 posTouch = Camera.main.ScreenToWorldPoint(touch.position);
            RootCameraAngleCharacter(posTouch);
        }
#endif

    }

    private void RootCameraAngleCharacter (Vector3 point)
    {
        float distancePoint = Vector2.Distance(point, transform.position);


        if (distancePoint > 0f)
        {
            Vector3 direction = new Vector3();


            direction.x = point.x - transform.position.x;
            direction.y = point.y - transform.position.y;


            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            _numberCameraAngle = _cameraAngles.FindIndex(x => angle >= x.MinAngle && angle <= x.MaxAngle);

            SetCameraAngleSprite();
        }

    }

    private void SetActiveCharacter(bool status)
    {
        _move = status;

        OnMoveStatusChanged?.Invoke(_move);
    }


    private void SetWorldPositionMove (Vector2 position)
    {     
        _positionMove = Camera.main.ScreenToWorldPoint(position);

        OnMove?.Invoke(_positionMove);
    }

    private void SetPositionMove (Vector2 pos) => _positionMove = pos;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == TAG_WALL_BLOCK)
        {
            SetActiveCharacter(false);
        }
    }


    private void SetStateMoveCharacter (bool status)
    {
        _isActiveMove = status;

        if (_move && !status)
        {
            _move = status;

            SetPositionMove(transform.position);
        }
    }

    public void Disable ()
    {
        if (!_move)
        {
       SetStateMoveCharacter(false);
        }
 
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {   
        if (stream.IsWriting)
        {
            stream.SendNext(_numberCameraAngle);
        }

        else
        {
            Ini();

          _numberCameraAngle = (int)stream.ReceiveNext();
          SetCameraAngleSprite();

        }
    }

    public void SendRPC(Action action, RpcTarget target = RpcTarget.All, params object[] parameters) => _photonView.RPC(action.Method.Name, target, parameters);

    public void SendSecureRPC(Action action, RpcTarget target = RpcTarget.All, bool encrypt = true, params object[] parameters) => _photonView.RpcSecure(action.Method.Name, target, encrypt, parameters);

    public void CallInvokingEveryMethod(Action method, float time) => InvokeRepeating(method.Method.Name, time, time);

    public void CallInvokingMethod(Action method, float time) =>  Invoke(method.Method.Name, time);

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Contains(TAG_PLAYER))
        {
            _body2d.velocity = Vector2.zero;
        }
    }

    private void SetCameraAngleSprite() => SetSprite(_characterCameraAnglesSettings.CameraAngles[_numberCameraAngle]);
    
    private void StartMoving() => SetActiveCharacter(true);
    
    public void Enable() => SetStateMoveCharacter(true);

    public void Disable(float timeOut) => CallInvokingMethod(Disable, timeOut);

    public void Enable(float timeOut) => CallInvokingMethod(Enable, timeOut);

    public void SetSprite(Sprite sprite) => _spriteRenderer.sprite = sprite;
       
}
