using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<string> teams;
    public static GameManager instance;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

}