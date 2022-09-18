using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera[] cameras;
    // Start is called before the first frame update
    void Start()
    {
        cameras = FindObjectsOfType<Camera>();
        DontDestroyOnLoad(gameObject);
    }

    public void SwitchToCameraWithTag(string tag)
    {
        foreach (var cam in cameras)
            cam.enabled = CompareTag(tag);
    }
    
    public Camera GetCamera(string tag)
    {
        foreach (var cam in cameras)
            if (cam.CompareTag(tag))
                return cam;
        
        return cameras[0];
    }
}
