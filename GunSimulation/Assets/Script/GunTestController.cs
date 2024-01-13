using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTestController : MonoBehaviour
{
    // Start is called before the first frame update
    private int sign = 1;
    private bool isClicked = false;
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
    void Update()
    {
        if(isClicked)
        { 
            transform.position = new Vector3 (Mathf.Clamp(transform.position.x - Time.deltaTime * sign, 0.5f, 2.0f) , 0, 0);
        }
        
    }
}
