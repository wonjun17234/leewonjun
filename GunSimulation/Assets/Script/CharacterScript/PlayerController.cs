using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : Character
{
    Vector3 dir;

    CharacterController characterController;
    public float speed; //ĳ���� �ӵ�
    public float JumpPower; //���� �Ŀ�
    public float currentJump;

    public float gravity; //�߷�
    public float gravityOffset;

    public float verticalSpeed; //���� �Ʒ��������� �ް��ִ� ���ӵ�
    
    public bool isGround; // ���� ������



    public GameObject direction; //�÷��̾ �ٶ󺸰� �ִ� ����

    public float rotationYSpeed = 3f;
    public float rotationXSpeed = 5f;
    public bool bcursor;
    
    public GameObject Camera;

    public GameObject body;
    public PlayerWeapon weapon;
    public GameObject grenade;

    private float eulerAngleX;
    private float eulerAngleY;

    public GameObject manager;

    public Transform gunPosition;

    // ray�� ����
    [SerializeField]
    private float _maxDistance = 0.5f;

    // ray�� ����
    [SerializeField]
    private Color _rayColor = Color.black;

    // Start is called before the first frame update
    void Awake()
    {
        UnityEngine.Cursor.visible = bcursor;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        gravity *= gravityOffset;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        ApplyGravity();
        RotatePlayer();

        int layerMask = 1 << LayerMask.NameToLayer("interaction");

        RaycastHit[] sphereHit = Physics.SphereCastAll(transform.position, 3, -transform.up, 5, layerMask);

        if (!manager.GetComponent<ScoreManager>().isOptionOn)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1)) 
            {
                weapon.gameObject.SetActive(true);
                grenade.SetActive(false);
            }

            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                weapon.gameObject.SetActive(false);
                grenade.SetActive(true);
            }
    
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                weapon.Reload();
            }

            if(Input.GetKeyDown(KeyCode.F))
            {

                if (sphereHit.Length != 0)
                {
                    
                    int minIndex = 0;
                    float minDist = Vector3.Distance(transform.position, sphereHit[minIndex].transform.position);
                    
                    for (int i = 1; i < sphereHit.Length; i++)
                    {
                        if(minDist > Vector3.Distance(transform.position, sphereHit[i].transform.position))
                        {
                            minDist = Vector3.Distance(transform.position, sphereHit[i].transform.position);
                            minIndex = i;
                        }
                    }
                    GameObject temp = sphereHit[minIndex].transform.gameObject;
                    Destroy(temp.GetComponent<Rigidbody>());
                    temp.transform.parent = gunPosition.transform;
                    temp.transform.localPosition = Vector3.zero;
                    temp.transform.localRotation = new Quaternion(0,0,0,0);
                    
                }
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



    private void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        eulerAngleY += mouseX * rotationYSpeed;
        eulerAngleX -= mouseY * rotationXSpeed;

        eulerAngleX = ClampAngle(eulerAngleX, -90f, 90f);
        transform.rotation = Quaternion.Euler(0, eulerAngleY, 0);
        Camera.transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY - 50f, 0);
    }
    

    private void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        dir = direction.transform.right * x * speed + direction.transform.forward * z * speed;
        
    }

    private void ApplyGravity()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Enviroment");
        SetGround(Physics.BoxCast(transform.position + transform.up * 0.5f, transform.lossyScale / 2.0f, -transform.up, out RaycastHit hit, transform.rotation, _maxDistance, layerMask));
        verticalSpeed += gravity * Time.deltaTime;
        if (isGround)
            verticalSpeed = 0;

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            verticalSpeed = JumpPower;
        }   

        dir.y = verticalSpeed;
        characterController.Move(dir * Time.deltaTime);
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
    void OnDrawGizmos()
    {
        Gizmos.color = _rayColor;

        // �Լ� �Ķ���� : ���� ��ġ, Box�� ���� ������, Ray�� ����, RaycastHit ���, Box�� ȸ����, BoxCast�� ������ �Ÿ�
        if (Physics.BoxCast(transform.position + transform.up * 0.5f, transform.lossyScale / 2.0f, -transform.up, out RaycastHit hit, transform.rotation, _maxDistance))
        {
            // Hit�� �������� ray�� �׷��ش�.
            Gizmos.DrawRay(transform.position + transform.up * 0.5f, -transform.up * hit.distance);

            // Hit�� ������ �ڽ��� �׷��ش�.
            Gizmos.DrawWireCube(transform.position + transform.up * 0.5f - transform.up * hit.distance, transform.lossyScale);
        }
        else
        {
            // Hit�� ���� �ʾ����� �ִ� ���� �Ÿ��� ray�� �׷��ش�.
            Gizmos.DrawRay(transform.position + transform.up * 0.5f, -transform.up * _maxDistance);
        }
    }

}
