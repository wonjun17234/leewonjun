using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera camera1;
    public Camera camera3;
    public bool isCamera3 = true;
    private void Awake()
    {
        camera1.enabled = false;
        camera3.enabled = true;
    }
    public void changeCamera()
    {
        camera1.enabled = isCamera3;
        isCamera3 = !isCamera3;
        camera3.enabled = isCamera3;
        
    }
}
