using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class GunTestController : MonoBehaviour
{
    public float RotationSpeed;
    public float MoveSpeed;
    private int sign = 1;
    private bool isClicked = false;

    private Vector2 PriPos;

    private Vector2 currentPos;
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
        
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if(isClicked)
        {
            if (Vector3.Distance(Vector3.zero, transform.position) > 0.9f && Vector3.Distance(Vector3.zero, transform.position) < 2.0f) //사이 값이면 마음대로 움직이게
            {
                transform.position = transform.position + transform.forward * sign * MoveSpeed;
            }
            else if (Vector3.Distance(Vector3.zero, transform.position) <= 0.9f && sign < 0) //너무 앞이면 뒤로만 갈 수 있게
            {
                transform.position = transform.position + transform.forward * sign * MoveSpeed;
            }
            else if (Vector3.Distance(Vector3.zero, transform.position) >= 2.0f && sign > 0) //너무 뒤면 앞으로만 갈 수 있게
            {
                transform.position = transform.position + transform.forward * sign * MoveSpeed;
            }            
        }
        
        if (Input.GetMouseButton(1))
        {
            //공전 구현
        
        }
        


    }
}
