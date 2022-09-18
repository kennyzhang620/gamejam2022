using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gradual_Intensity : MonoBehaviour
{
    Light light;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (light.intensity <= 4)
        {
            light.intensity += Time.deltaTime;
        }
    }
}
