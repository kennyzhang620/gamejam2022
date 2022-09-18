using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    //State
    private Vector3 targetDestination;
    public AsteroidState State;
    private bool stateUpdate;
    private Rigidbody _rigidbody;
    private float speed;
    private float initialDistance;
    private Vector3 rotDir;

    //variables for randomization limits
    public float maxRandSpeed = 1;
    public float maxRandDist = 100;
    
    //relating to targeting and hitting the ship
    private GameObject target;

    public enum AsteroidState
    {
        Start,
        Idle,
        IdleMove,
        TargetLocked,
        Collision
    }

    void Awake()
    {
        //set the constant speed of the asteroid
        //this is random and done only once
        speed = Random.Range(0, maxRandSpeed);
        _rigidbody = GetComponent<Rigidbody>();
        stateUpdate = true;
        State = AsteroidState.IdleMove;
        rotDir = Vector3.zero;
        initialDistance = 0;
        rotDir.x = Random.Range(0, 1f);
        rotDir.y = Random.Range(0, 1f);
        rotDir.z = Random.Range(0, 1f);
    }

    public void SetTarget(GameObject obj)
    {
        if(target!=null)
            //consider adding type check here 
            //targets should be either ship or asteroid
            target = obj;
    }
    
    //time till astroid reached/impacts its destination 
    public float GetTimeToImpact()
    {
        return GetDistanceToImpact() / speed;
    }

    public float GetDistanceToImpact()
    {
        return (targetDestination - _rigidbody.position).magnitude;
    }
    

    //track ship position and slerp move asteroid to it
    private void TargetLock()
    {
        targetDestination = target.transform.position;
        if (State == AsteroidState.TargetLocked &&
            stateUpdate)
        {
            initialDistance = GetDistanceToImpact();
            stateUpdate = false;
        }
        
        SlerpMove();
    }

    private void SetRandomDestination()
    {
        targetDestination = _rigidbody.position;
        targetDestination.x += Random.Range(-1*maxRandDist, maxRandDist);
        targetDestination.y += Random.Range(-1*maxRandDist, maxRandDist);
        targetDestination.z += Random.Range(-1*maxRandDist, maxRandDist);
    }

    //idle the astroid, 0 velocity, only rotates in space
    private void Idle()
    {
        if (stateUpdate)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = rotDir * speed;
            stateUpdate = false;
        }
    }

    //move astroid to destination by spherical interpolation to randomize path
    private void SlerpMove()
    {
        if (!_rigidbody.position.Equals(targetDestination))
        {
            // float t =  speed/(destination - _rigidbody.position).magnitude;
            float t = (initialDistance-GetDistanceToImpact())/initialDistance;
            if (t <= 0)
                t = 0.05f;
            Vector3 nextPos = Vector3.Slerp(_rigidbody.position, targetDestination, t);
            Vector3 dir = (nextPos - _rigidbody.position).normalized;
            _rigidbody.velocity = dir * speed;
        }
    }

    //move to a random destination initially set after state switch
    private void IdleMove()
    {
        if (stateUpdate)
        {
            _rigidbody.angularVelocity = rotDir * speed;
            SetRandomDestination();
            initialDistance = GetDistanceToImpact();
            stateUpdate = false;
        }
        
        SlerpMove();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Ship")
        {
            State = AsteroidState.Collision;
            stateUpdate = true;
        } else if (collision.gameObject.GetComponent<Asteroid>() != null)
        {
            //chain reaction 
        }
    }

    private void AsteroidCollisionEvent()
    {
        //do explosion/impact animation here
    }

    void FixedUpdate()
    {
        if (State != AsteroidState.Collision)
        {
            if (targetDestination.Equals(_rigidbody.position))
            {
                stateUpdate = true;
                State = AsteroidState.IdleMove;
            }

            if(State==AsteroidState.Idle)
                Idle();
            else if(State==AsteroidState.IdleMove)
                IdleMove();
            else if(State==AsteroidState.TargetLocked)
                TargetLock();
        }
        else
        {
            AsteroidCollisionEvent();
            enabled = false;
            Destroy(this);
        }
    }
}
