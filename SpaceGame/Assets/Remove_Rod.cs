using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remove_Rod : MonoBehaviour
{
    public Spawner sp;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other);
        sp.Spawned = false;
    }
}
