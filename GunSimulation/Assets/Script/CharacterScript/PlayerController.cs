using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : Character
{
    Vector3 dir;

    CharacterController characterController;
    public float speed; //캐릭터 속도
    public float JumpPower; //점프 파워
    public float currentJump;

    public float gravity; //중력
    public float gravityOffset;

    public float verticalSpeed; //현재 아래방향으로 받고있는 가속도
    
    public bool isGround; // 현재 땅인지
    public bool isJumping;


    public float time;
    public GameObject direction; //플레이어가 바라보고 있는 방향

    public float rotationYSpeed = 3f;
    public float rotationXSpeed = 5f;
    public bool bcursor;
    
    public GameObject Camera;

    public GameObject body;
    public PlayerWeapon weapon;
    private float eulerAngleX;
    private float eulerAngleY;

    public GameObject manager;


    // ray의 길이
    [SerializeField]
    private float _maxDistance = 0.5f;

    // ray의 색상
    [SerializeField]
    private Color _rayColor = Color.black;

    // Start is called before the first frame update
    void Awake()
    {
        UnityEngine.Cursor.visible = bcursor;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        time = 0;
        gravity *= gravityOffset;
    }

    // Update is called once per frame
    void Update()
    {
        SetGround(Physics.BoxCast(transform.position + transform.up * 0.5f, transform.lossyScale / 2.0f, -transform.up, out RaycastHit hit, transform.rotation, _maxDistance));
        time += Time.deltaTime;
        verticalSpeed += gravity * Time.deltaTime;
        if (isGround)
            verticalSpeed = 0;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float y;
        dir = new Vector3(x, 0, z) * speed;
        if (isJumping)
        {
            currentJump = Mathf.Lerp(0, JumpPower, time * 4);
            if(currentJump == JumpPower)
            {
                isJumping = false;
                currentJump = 0;
            }
        }
        dir.y = currentJump - verticalSpeed;
        
        dir = direction.transform.TransformDirection(dir);

        characterController.Move(dir * Time.deltaTime);

        if (!manager.GetComponent<ScoreManager>().isOptionOn)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            eulerAngleY += mouseX * rotationYSpeed;
            eulerAngleX -= mouseY * rotationXSpeed;

            eulerAngleX = ClampAngle(eulerAngleX, -90f, 90f);
            transform.rotation = Quaternion.Euler(0, eulerAngleY, 0);
            Camera.transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY - 50f, 0);

            if (Input.GetKeyDown(KeyCode.Space) && isGround)
            {
                time = 0;
                isJumping = true;
            }
            
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                weapon.Reload();
            }

            if (Input.GetKey(KeyCode.G))
            {
                weapon.Drop();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            manager.GetComponent<ScoreManager>().ClickOptionOut();
        }
        


    }

    


    void OnDrawGizmos()
    {
        Gizmos.color = _rayColor;

        // 함수 파라미터 : 현재 위치, Box의 절반 사이즈, Ray의 방향, RaycastHit 결과, Box의 회전값, BoxCast를 진행할 거리
        if (Physics.BoxCast(transform.position + transform.up * 0.5f, transform.lossyScale / 2.0f, -transform.up, out RaycastHit hit, transform.rotation, _maxDistance))
        {
            // Hit된 지점까지 ray를 그려준다.
            Gizmos.DrawRay(transform.position + transform.up * 0.5f, -transform.up * hit.distance);

            // Hit된 지점에 박스를 그려준다.
            Gizmos.DrawWireCube(transform.position + transform.up * 0.5f - transform.up * hit.distance, transform.lossyScale);
        }
        else
        {
            // Hit가 되지 않았으면 최대 검출 거리로 ray를 그려준다.
            Gizmos.DrawRay(transform.position + transform.up * 0.5f, -transform.up * _maxDistance);
        }
    }

    private void SetGround(bool value)
    {
        isGround = value;
    }
    private float ClampAngle(float angle, float min, float max)
    {
        if(angle < -360) angle += 360;
        if(angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    
}
