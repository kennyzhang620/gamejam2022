using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera[] cameras;
    // Start is called before the first frame update

    private void Awake()
    {
        cameras = FindObjectsOfType<Camera>();
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SwitchToCameraWithTag(string tag)
    {
        foreach (var cam in cameras)
            cam.enabled = cam.CompareTag(tag);
    }
    
    public Camera GetCamera(string tag)
    {
        foreach (var cam in cameras)
            if(cam != null && cam.CompareTag(tag))
                return cam;
        return cameras[0];
    }
}
