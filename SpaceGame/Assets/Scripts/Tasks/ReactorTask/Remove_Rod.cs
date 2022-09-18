using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remove_Rod : MonoBehaviour
{
    public ReactorTaskSpawner sp;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other);
        sp.Spawned = false;
    }
}
