using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    public float rotationYSpeed = 3f;
    public float rotationXSpeed = 5f;
    public bool bcursor = false;
    public float moveSpeed = 1f;
    public float JumpPower = 1f;

    private float eulerAngleX;
    private float eulerAngleY;
    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 60;
        UnityEngine.Cursor.visible = bcursor;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        eulerAngleY += mouseX * rotationYSpeed;
        eulerAngleX -= mouseY * rotationXSpeed;

        eulerAngleX = ClampAngle(eulerAngleX, -90f, 90f);
        transform.parent.rotation = Quaternion.Euler(0, eulerAngleY, 0);
        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);

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

    private float ClampAngle(float angle, float min, float max)
    {
        if(angle < -360) angle += 360;
        if(angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
