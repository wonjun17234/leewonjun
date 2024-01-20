using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera camera1;
    public Camera camera3;

    public GameObject pointOfView1;
    public GameObject pointOfView3;
    public bool isCamera3 = true;
    private void Awake()
    {
        camera1.enabled = false;
        camera3.enabled = true;
        pointOfView1.SetActive(false);
        pointOfView3.SetActive(true);
    }
    public void changeCamera()
    {
        camera1.enabled = isCamera3;
        pointOfView1.SetActive(isCamera3);
        isCamera3 = !isCamera3;
        camera3.enabled = isCamera3;
        pointOfView3.SetActive(isCamera3);
    }
}
