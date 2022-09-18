using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera[] cameras;
    // Start is called before the first frame update
    void Awake()
    {
        cameras = FindObjectsOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

        foreach (var cam in cameras)
        {
            //do camera switching here

        }
        
    }
}
