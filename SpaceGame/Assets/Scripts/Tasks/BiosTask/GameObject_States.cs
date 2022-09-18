using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObject_States : MonoBehaviour
{
    public GameObject Enable;
    public GameObject Disable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Activate()
    {
        Enable.SetActive(true);
        Disable.SetActive(false);
    }
}
