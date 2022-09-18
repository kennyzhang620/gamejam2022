using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    //State
    private Vector3 destination;
    public AsteroidState State { get; set; }
    private AsteroidState lastState;
    private Rigidbody _rigidbody;
    private float speed;
    private Vector3 rotDir;
    
    //variables for randomization limits
    public static float maxRandSpeed = 5;
    public static float maxRandDist = 100;
    
    //relating to targeting and hitting the ship
    private Ship targetShip;
    private bool hitShip;
    
    public enum AsteroidState
    {
        Idle,
        IdleMove,
        TargetLocked,
        Collide,
        Destroyed
    }

    void Awake()
    {
        //set the constant speed of the asteroid
        //this is random and done only once
        speed = Random.Range(0, maxRandSpeed);
        lastState = AsteroidState.Idle;
        State = AsteroidState.Idle;
        rotDir = Vector3.zero;
        rotDir.x = Random.Range(0, 1f);
        rotDir.y = Random.Range(0, 1f);
        rotDir.z = Random.Range(0, 1f);
        targetShip = FindObjectOfType<Ship>();
    }
    
    //time till astroid reached/impacts its destination 
    float GetTimeToImpact()
    {
        return (destination - _rigidbody.position).magnitude / speed;
    }

    //track ship position and slerp move asteroid to it
    void TargetLock()
    {
        destination = targetShip.transform.position;
        SlerpMove();
    }

    //idle the astroid, 0 velocity, only rotates in space
    void Idle()
    {
        if (lastState != AsteroidState.Idle)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = rotDir * speed;
        }
    }

    //move astroid to destination by spherical interpolation to randomize path
    void SlerpMove()
    {
        if (!_rigidbody.position.Equals(destination))
        {
            float t =  speed/(destination - _rigidbody.position).magnitude;
            Vector3 nextPos = Vector3.Slerp(_rigidbody.position, destination, t);
            _rigidbody.MovePosition(nextPos);
        }
    }

    //move to a random destination initially set after state switch
    void IdleMove()
    {
        if (lastState != AsteroidState.IdleMove)
        {
            _rigidbody.angularVelocity = rotDir * speed;
            destination = _rigidbody.position;
            destination.x += Random.Range(0, maxRandDist);
            destination.y += Random.Range(0, maxRandDist);
            destination.z += Random.Range(0, maxRandDist);
        }
        
        SlerpMove();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Ship")
        {
            //do something when asteroid hits
            //also update asteroid states
        }
    }

    void Update()
    {
        if(State==AsteroidState.Idle)
            Idle();
        else if(State==AsteroidState.IdleMove)
            IdleMove();
        else if(State==AsteroidState.TargetLocked)
            TargetLock();
        
        
        
        



        lastState = State;

    }
}
