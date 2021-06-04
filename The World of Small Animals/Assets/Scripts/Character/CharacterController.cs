using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class CharacterController : MonoBehaviour, ISeterSprite, IPunObservable, IRPCSender, IInvokerMono
{
    private const string PATH_SETTINGS_CHARACTER = "Data/Character/CharacterSettings";
    private const string TAG_WALL_BLOCK = "WallBlock";
    private const string TAG_PLAYER = "Player";
    [Header("��������� ��������")]
    [SerializeField]
    private CharacterCameraAngles characterCameraAnglesSettings;

    private Rigidbody2D body2d;

    [Header("������� �������� ���������")]
    [ReadOnlyField]
    [SerializeField]
    private float currentSpeed = 0f;
    [Header("�������� ��������� ��� ������")]
    [ReadOnlyField]
    [SerializeField]
    private float startedSpeed = 0f;

    Vector2 posMove;


    [Header("��������")]
    [ReadOnlyField]
    [SerializeField]
    private bool move = false;


    [Header("���������")]
    [ReadOnlyField]
    [SerializeField]
    private bool rotating = true;

    [Header("����� ��������� �������")]
    [ReadOnlyField]
    [SerializeField]
    private int numberCameraAngle = 0;

    private bool initialized = false;




    [Header("�������")]
    [ReadOnlyField]
    [SerializeField]
    private bool activeMove = true;

    private CharacterSettings characterSettings;

    private Camera cameraMain;

    private PhotonView photonView;

    private SpriteRenderer spriteRenderer;

    public event Action<Vector3> onMove;

    public event Action<bool> onMoveStatusChanged;

    // rotation

    private List<TupleAngle> cameraAngles =


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


    public bool ActiveMove { get => activeMove; }
    public float CurrentSpeed { get => currentSpeed; }
    public bool Moved { get => move; }
    public CharacterCameraAngles CharacterCameraAnglesSettings { get => characterCameraAnglesSettings; }




    // Start is called before the first frame update
    private void Start()
    {
        Ini();

        startedSpeed = characterSettings.Speed;

        currentSpeed = startedSpeed;

        cameraMain = Camera.main;

    }

    private void Awake()
    {
        if (!TryGetComponent(out spriteRenderer))
        {
            throw new CharacterException($"{name} not have component Sprite Renderer");
        }

        if (!TryGetComponent(out photonView))
        {
            throw new CharacterException($"{name} not have component Photon View");
        }

        name = photonView.Owner.NickName;

        tag = photonView.IsMine ? $"My{TAG_PLAYER}" : TAG_PLAYER;

       
    }

    private void Ini()
    {

        if (initialized)
        {
            return;
        }


        if (characterCameraAnglesSettings == null)
        {
            throw new CharacterException("character camera angles not seted");
        }


        if (!TryGetComponent(out body2d))
        {
            throw new CharacterException($"{name} not have component Rightbody2D");
        }

        characterSettings = Resources.Load<CharacterSettings>(PATH_SETTINGS_CHARACTER);

        if (characterSettings == null)
        {
            throw new CharacterException("character settings not found");
        }

        initialized = true;
    }

    private void Control ()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0) == true && activeMove)
        {
            if (!move)
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

    void Update()
    {
        if (photonView.IsMine)
        {
        	body2d.velocity = Vector2.zero;
            Control();
        }

    }

    void FixedUpdate () {
    	        if (photonView.IsMine)
        {
        	body2d.velocity = Vector2.zero;
        }
    }

    private void StartMoving()
    {
        SetActiveCharacter(true);
    }

    private void CheckMove()
    {

        if (!activeMove)
        {
            return;
        }

       

        if (move)
        {
            Vector3 targetPosMove = Vector2.MoveTowards(transform.position, posMove, currentSpeed * Time.deltaTime);

            body2d.MovePosition(targetPosMove);
            

            if (Vector2.Distance(transform.position, posMove) <= 0.01f)
            {
                SetActiveCharacter(false);
               
            }
        }
    }

    private void CheckRotation ()
    {
        if (!activeMove)
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

            numberCameraAngle = cameraAngles.FindIndex(x => angle >= x.MinAngle && angle <= x.MaxAngle);

            SetCameraAngleSprite();
        }

    }

    private void SetActiveCharacter(bool status)
    {
        move = status;
        rotating = !status;

        onMoveStatusChanged?.Invoke(move);

    }


    private void SetWorldPositionMove (Vector2 pos)
    {     
        posMove = Camera.main.ScreenToWorldPoint(pos);

        onMove?.Invoke(posMove);
    }

    private void SetPositionMove (Vector2 pos)
    {
        posMove = pos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == TAG_WALL_BLOCK)
        {
            SetActiveCharacter(false);
        }
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    private void SetStateMoveCharacter (bool status)
    {
        activeMove = status;

        if (move && !status)
        {
            move = status;
            SetPositionMove(transform.position);
        }
    }

    public void Disable ()
    {
        if (!move)
        {
       SetStateMoveCharacter(false);
        }
 
    }

    public void Enable ()
    {
        SetStateMoveCharacter(true);
    }

    public void Disable(float timeOut)
    {
        CallInvokingMethod(Disable, timeOut);
    }

    public void Enable(float timeOut)
    {
        CallInvokingMethod(Enable, timeOut);
    }

    private void SetCameraAngleSprite()
    {
        SetSprite(characterCameraAnglesSettings.CameraAngles[numberCameraAngle]);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
        if (stream.IsWriting)
        {
            stream.SendNext(numberCameraAngle);
        }

        else
        {
            Ini();


            numberCameraAngle = (int)stream.ReceiveNext();
          SetCameraAngleSprite();


        }
    }

    public void SendRPC(Action action, RpcTarget target = RpcTarget.All, params object[] parameters)
    {
        photonView.RPC(action.Method.Name, target, parameters);
    }

    public void SendSecureRPC(Action action, RpcTarget target = RpcTarget.All, bool encrypt = true, params object[] parameters)
    {
        photonView.RpcSecure(action.Method.Name, target, encrypt,  parameters);
    }

    public void CallInvokingEveryMethod(Action method, float time)
    {
        InvokeRepeating(method.Method.Name, time, time);
    }

    public void CallInvokingMethod(Action method, float time)
    {
        Invoke(method.Method.Name, time);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Contains(TAG_PLAYER))
        {
            body2d.velocity = Vector2.zero;
        }
    }
}
