using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Missile : MonoBehaviour
{
    [SerializeField] private float speed = 100f;

    [SerializeField] private GameObject explosionPrefab;

    private Rigidbody _rigidbody;
    public bool destroyed;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = MainGame.Instance.PlayerShip.Velocity * speed;
        destroyed = false;
        // _rigidbody.angularVelocity = new Vector3(0, 1, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        destroyed = true;
    }
}
