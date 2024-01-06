using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    public float RotationSpeed = 1f;
    public bool bcursor = true;
    public float moveSpeed = 1f;
    public float JumpPower = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        UnityEngine.Cursor.visible = bcursor;
    }

    Vector3 prevRot = Vector3.zero; 
    // Update is called once per frame
    void Update()
    {
        Vector3 mouse = Input.mousePosition;

        Vector3 target = new Vector3(0, mouse.x - 550, 0);
        transform.parent.localEulerAngles = target * RotationSpeed;
        target = new Vector3(Mathf.Clamp(-mouse.y + 260, -90f, 90f), 0, 0);
        transform.localEulerAngles = target * RotationSpeed;
        
        if (Input.GetKey(KeyCode.W))
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
