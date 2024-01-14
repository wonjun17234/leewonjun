using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class GunTestController : MonoBehaviour
{
    public float Yaxis;
    public float Xaxis;

    public GameObject camera;

    public float RotationSpeed;
    public float MoveSpeed;
    public float MaxDistance;
    public float MinDistance;
    public float currentDistance;
    private int sign = 1; //+버튼인지 -버튼인지

    private bool isClicked = false;

    private Vector3 targetRotation;
    private Vector3 currentVel;


    private Vector3 mousePos_prev;
    public void ButtonDown(int num)
    {
        sign *= num;
        isClicked = true;
    }

    public void ButtonUp()
    {
        sign = 1;
        isClicked = false;
    }

    void Start()
    {
        //mousePos_prev = Input.mousePosition;
        currentDistance = Vector3.Distance(Vector3.zero, transform.position);
        targetRotation = new Vector3(0, Yaxis, 0);
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(camera.GetComponent<CameraController>().isCamera3)
        {
            if (isClicked)
            {
                currentDistance = Vector3.Distance(Vector3.zero, transform.position);
                if (currentDistance > MinDistance && currentDistance < MaxDistance) //사이 값이면 마음대로 움직이게
                {
                    transform.position = transform.position + transform.forward * sign * MoveSpeed;
                }
                else if (currentDistance <= MinDistance && sign < 0) //너무 앞이면 뒤로만 갈 수 있게
                {
                    transform.position = transform.position + transform.forward * sign * MoveSpeed;
                }
                else if (currentDistance >= MaxDistance && sign > 0) //너무 뒤면 앞으로만 갈 수 있게
                {
                    transform.position = transform.position + transform.forward * sign * MoveSpeed;
                }
            }

            if (Input.GetMouseButton(1))
            {

                Vector3 mouseDelta = Input.mousePosition - mousePos_prev;
                Yaxis = Yaxis + mouseDelta.x * RotationSpeed;
                Xaxis = Xaxis - mouseDelta.y * RotationSpeed;
                Xaxis = ClampAngle(Xaxis, -90f, 90f);
                targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(Xaxis, Yaxis), ref currentVel, 0.12f);
                this.transform.eulerAngles = targetRotation;

                transform.position = Vector3.zero - transform.forward * currentDistance;


                mousePos_prev = Input.mousePosition;
            }
            else
            {
                mousePos_prev = Input.mousePosition;
            }
        }
        
        


    }
    private float ClampAngle(float angle, float min, float max)
    {
        return Mathf.Clamp(angle, min, max);
    }
}
