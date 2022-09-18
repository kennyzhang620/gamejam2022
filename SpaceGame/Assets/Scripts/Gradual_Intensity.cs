using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gradual_Intensity : MonoBehaviour
{
    Light light;
    Material mat;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        mat = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (light.intensity <= 4)
        {
            light.intensity += Time.deltaTime;
            mat.SetColor("_EmissionColor", Color.white*light.intensity);
        }

        
    }
}
