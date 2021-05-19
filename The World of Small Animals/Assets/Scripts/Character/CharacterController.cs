using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class CharacterController : MonoBehaviour, ISeterSprite
{
    private const string PATH_SETTINGS_CHARACTER = "Data/Character/CharacterSettings";


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
    private bool rotating = true;

    private bool activeMove = true;

    private CharacterSettings characterSettings;

    private Camera cameraMain;

    public bool ActiveMove { get => activeMove; }
    public float CurrentSpeed { get => currentSpeed; }

    // Start is called before the first frame update
    private void Start()
    {
        if (!TryGetComponent(out body2d))
        {
            throw new CharacterException($"{name} not have component Rightbody2D");
        }

        characterSettings = Resources.Load<CharacterSettings>(PATH_SETTINGS_CHARACTER);


        startedSpeed = characterSettings.Speed;
        
        SetCurrentSpeed(startedSpeed);

        cameraMain = Camera.main;
       
    }
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0) == true)
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

        if (Input.touchCount == 1)
        {
            Vector3 touchPos = Input.touches[0].position;
            SetWorldPositionMove(touchPos);
            StartMoving();
        }

#endif

        CheckMove();

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "WallBlock")
        {
            SetCurrentSpeed(startedSpeed);
            SetActiveCharacter(false);
        }
    }

    public void SetSprite(Sprite sprite)
    {
        throw new System.NotImplementedException();
    }
}
