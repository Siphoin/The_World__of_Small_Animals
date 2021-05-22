using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class CharacterController : MonoBehaviour, ISeterSprite, IPunObservable, IRPCSender
{
    private const string PATH_SETTINGS_CHARACTER = "Data/Character/CharacterSettings";
    private const string TAG_WALL_BLOCK = "WallBlock";

    [Header("Настройки ракурсов")]
    [SerializeField]
    private CharacterCameraAngles characterCameraAnglesSettings;

    private Rigidbody2D body2d;

    [Header("Текущая скорость персонажа")]
    [ReadOnlyField]
    [SerializeField]
    private float currentSpeed = 0f;
    [Header("Скорость персонажа при старте")]
    [ReadOnlyField]
    [SerializeField]
    private float startedSpeed = 0f;

    Vector2 posMove;


    [Header("Движется")]
    [ReadOnlyField]
    [SerializeField]
    private bool move = false;


    [Header("Вращается")]
    [ReadOnlyField]
    [SerializeField]
    private bool rotating = true;

    [Header("Номер активного ракурса")]
    [ReadOnlyField]
    [SerializeField]
    private int numberCameraAngle = 0;

    private bool initialized = false;




    [Header("Активен")]
    [ReadOnlyField]
    [SerializeField]
    private bool activeMove = true;

    private CharacterSettings characterSettings;

    private Camera cameraMain;

    private PhotonView photonView;


    public bool ActiveMove { get => activeMove; }
    public float CurrentSpeed { get => currentSpeed; }



    // Start is called before the first frame update
    private void Start()
    {
        Ini();

        startedSpeed = characterSettings.Speed;

        SetCurrentSpeed(startedSpeed);

        cameraMain = Camera.main;

    }

    private void Awake()
    {
        if (!TryGetComponent(out photonView))
        {
            throw new CharacterException($"{name} not have component Photon View");
        }

        name = photonView.Owner.NickName;

       
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
            Control();
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
                SetCurrentSpeed(startedSpeed);
            }
        }
    }

    private void CheckRotation ()
    {
        if (!activeMove || move)
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
            
            Vector3 posTouch = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
            RootCameraAngleCharacter(posTouch);
        }
#endif

    }

    private void RootCameraAngleCharacter (Vector3 point)
    {
        float distancePoint = Vector2.Distance(point, transform.position);


        if (distancePoint > 0f)
        {
        float angle = Mathf.Atan2(point.y - transform.position.y, point.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

    }

    private void SetActiveCharacter(bool status)
    {
        move = status;
        rotating = !status;
    }

    private void SetCurrentSpeed (float speed)
    {
        currentSpeed = speed;
    }

    private void SetWorldPositionMove (Vector2 pos)
    {     
        posMove = Camera.main.ScreenToWorldPoint(pos);
    }

    private void SetPositionMove (Vector2 pos)
    {
        posMove = pos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == TAG_WALL_BLOCK)
        {
            SetCurrentSpeed(startedSpeed);
            SetActiveCharacter(false);
        }
    }

    public void SetSprite(Sprite sprite)
    {
        throw new System.NotImplementedException();
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
        SetStateMoveCharacter(false);
    }

    public void Enable ()
    {
        SetStateMoveCharacter(true);
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
        //  SetCameraAngleSprite();


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
}
