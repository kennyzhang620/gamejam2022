using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    //State
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private float _explosionScale = 10f;
    private Vector3 targetDestination;
    private AsteroidState _state;

    public AsteroidState State
    {
        get { return _state; }
        set
        {
            stateUpdate = true;
            _state = value;
        }
    }
    private bool stateUpdate;
    private Rigidbody _rigidbody;
    private float speed;
    private float initialDistance;
    private Vector3 rotDir;

    //variables for randomization limits
    public float maxRandSpeed = 1;
    public float maxRandRotSpeed = 5;
    public float maxRandDist = 100;
    
    //relating to targeting and hitting the ship
    private GameObject target;
    public bool _isTargetShip;

    public enum AsteroidState
    {
        Start,
        Idle,
        IdleMove,
        TargetLocked,
        Collision,
        Destroyed
    }

    void Awake()
    {
        //set the constant speed of the asteroid
        //this is random and done only once
        speed = Random.Range(0, maxRandSpeed);
        _rigidbody = GetComponent<Rigidbody>();
        stateUpdate = true;
        _state = AsteroidState.IdleMove;
        rotDir = Vector3.zero;
        initialDistance = 0;
        rotDir.x = Random.Range(0, 1f);
        rotDir.y = Random.Range(0, 1f);
        rotDir.z = Random.Range(0, 1f);
        _isTargetShip = false;
    }

    public void SetTarget(GameObject obj)
    {
        if(obj != null)
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
        if (_state == AsteroidState.TargetLocked &&
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
        if (collision.gameObject.CompareTag("Ship") ||
            collision.gameObject.CompareTag("Missile"))
        {
            _state = AsteroidState.Collision;
        } else if (collision.gameObject.CompareTag("Asteroid"))
        {
            // do nothing for now
        }
    }

    private void AsteroidCollisionEvent()
    {
        //do explosion/impact animation here
        var explosion = Instantiate(_explosionPrefab, _rigidbody.position, Quaternion.identity);
        explosion.transform.localScale *=_explosionScale;
        State = AsteroidState.Destroyed;
    }

    public float Damage
    {
        get { return speed * transform.localScale.x; }
    }

    void FixedUpdate()
    {
        if (_state != AsteroidState.Collision && _state != AsteroidState.Destroyed)
        {
            if (targetDestination.Equals(_rigidbody.position))
            {
                stateUpdate = true;
                _state = AsteroidState.IdleMove;
            }

            if(_state==AsteroidState.Idle)
                Idle();
            else if(_state==AsteroidState.IdleMove)
                IdleMove();
            else if(_state==AsteroidState.TargetLocked)
                TargetLock();
        }
        else
        {
            AsteroidCollisionEvent();
        }
    }
}
