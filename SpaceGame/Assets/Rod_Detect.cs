using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rod_Detect : MonoBehaviour
{
    // Start is called before the first frame update
    public Spawner sp;
    public Slider sl;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        sp.Spawned = false;
        sl.value *= 0.90f;
    }
}
