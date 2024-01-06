using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    public float speed = 1f;
    public bool bcursor = true;
    public float moveSpeed = 1f;
    public float JumpPower = 1f;
    Vector3 initMouse;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        UnityEngine.Cursor.visible = bcursor;

        initMouse = Input.mousePosition;
    }

    Vector3 prevRot = Vector3.zero; 
    // Update is called once per frame
    void Update()
    {
        Vector3 mouse = Input.mousePosition - initMouse;

        Vector3 target = new Vector3(0, mouse.x, 0);
        transform.parent.localEulerAngles = target * speed;
        target = new Vector3(-mouse.y, 0, 0);
        transform.localEulerAngles = target * speed;


        if(Input.GetKey(KeyCode.W))
        {
            transform.parent.position += transform.parent.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.parent.position -= transform.parent.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.parent.position -= transform.parent.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.parent.position += transform.parent.right * moveSpeed * Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            transform.GetComponentInParent<Rigidbody>().AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
        }

        



    }
}
