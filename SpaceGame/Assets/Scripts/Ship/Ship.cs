using System;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    //Gameplay parameters
    private int asteroidImpacts;
    private int missileImpacts;

    public int AsteroidsDestroyed
    {
        get { return asteroidImpacts + missileImpacts; }
    }
    private List<Missile> launchedMissiles;
    [SerializeField] private GameObject _missilePrefab;

    //INPUT VARIABLES 
    private Camera _shipCamera;
    [SerializeField] private Vector3 inertiaTensor = Vector3.zero;
    [SerializeField] private Quaternion inertiaTensorRotation = Quaternion.identity;
    [SerializeField] private float bankLimit = 35f;
    [SerializeField] private float pitchSensitivity = 2.5f;
    [SerializeField] private float yawSensitivity = 2.5f;
    [SerializeField] private float rollSensitivity = 1f;
    [Range(-1, 1)]
    [SerializeField] private float pitch;
    [Range(-1, 1)]
    [SerializeField] private float yaw;
    [Range(-1, 1)]
    [SerializeField] private float roll;
    [Range(-1, 1)]
    [SerializeField] private float strafe;
    [Range(0, 1)]
    [SerializeField] private float throttle;
    private const float THROTTLE_SPEED = 0.5f;
    public float Pitch { get { return pitch; } }
    public float Yaw { get { return yaw; } }
    public float Roll { get { return roll; } }
    public float Strafe { get { return strafe; } }
    public float Throttle { get { return throttle; } }

    //PHYSICS VARIABLES
    private Rigidbody _rigidbody;
    public Vector3 Velocity { get { return _rigidbody.velocity; } }
    public Vector3 linearForce = new Vector3(100.0f, 100.0f, 100.0f);
    public Vector3 angularForce = new Vector3(100.0f, 100.0f, 100.0f);
    [Range(0.0f, 1.0f)]
    public float reverseMultiplier = 1.0f;
    public float forceMultiplier = 100.0f;
    private Vector3 appliedLinearForce = Vector3.zero;
    private Vector3 appliedAngularForce = Vector3.zero;
    void Awake()
    {
        asteroidImpacts = 0;
        missileImpacts = 0;
        launchedMissiles = new List<Missile>();
        _rigidbody = GetComponent<Rigidbody>();
        // _rigidbody.inertiaTensorRotation = inertiaTensorRotation;
        // _rigidbody.inertiaTensor = inertiaTensor;

    }

    //input related updates
    void Update()
    {
        strafe = Input.GetAxis("Horizontal");

        SetStickCommandsUsingAutopilot();

        UpdateMouseWheelThrottle();
        UpdateKeyboardThrottle(KeyCode.W, KeyCode.S);
        SetPhysicsInput(new Vector3(strafe, 0.0f, throttle), new Vector3(pitch, yaw, roll));
        launchMissiles();
    }

    //physics related updates
    void FixedUpdate()
    {
        if (_rigidbody != null)
        {
            _rigidbody.AddRelativeForce(appliedLinearForce * forceMultiplier, ForceMode.Force);
            _rigidbody.AddRelativeTorque(appliedAngularForce * forceMultiplier, ForceMode.Force);
        }
    }
    
    private void SetStickCommandsUsingAutopilot()
    {
        // Project the position of the mouse on screen out to some distance.
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 1000f;
        Vector3 gotoPos = MainGame.Instance.CameraController.GetCamera("ShipCamera").ScreenToWorldPoint(mousePos);

        // Use that world position under the mouse as a target point.
        TurnTowardsPoint(gotoPos);

        // Use the mouse to bank the ship some degrees based on the mouse position.
        BankShipRelativeToUpVector(mousePos, MainGame.Instance.CameraController.GetCamera("ShipCamera").transform.up);
    }
    
    private void BankShipRelativeToUpVector(Vector3 mousePos, Vector3 upVector)
    {
        float bankInfluence = (mousePos.x - (Screen.width * 0.5f)) / (Screen.width * 0.5f);
        bankInfluence = Mathf.Clamp(bankInfluence, -1f, 1f);

        // Throttle modifies the bank angle so that when at idle, the ship just flatly yaws.
        bankInfluence *= throttle;
        float bankTarget = bankInfluence * bankLimit;

        // Here's the special sauce. Roll so that the bank target is reached relative to the
        // up of the camera.
        float bankError = Vector3.SignedAngle(transform.up, upVector, transform.forward);
        bankError = bankError - bankTarget;

        // Clamp this to prevent wild inputs.
        bankError = Mathf.Clamp(bankError * 0.1f, -1f, 1f);

        // Roll to minimze error.
        roll = bankError * rollSensitivity;
    }
    
    private void TurnTowardsPoint(Vector3 gotoPos)
    {
        Vector3 localGotoPos = transform.InverseTransformVector(gotoPos - transform.position).normalized;

        // Note that you would want to use a PID controller for this to make it more responsive.
        pitch = Mathf.Clamp(-localGotoPos.y * pitchSensitivity, -1f, 1f);
        yaw = Mathf.Clamp(localGotoPos.x * yawSensitivity, -1f, 1f);
    }
    
    private void UpdateKeyboardThrottle(KeyCode increaseKey, KeyCode decreaseKey)
    {
        float target = throttle;

        if (Input.GetKey(increaseKey))
            target = 1.0f;
        else if (Input.GetKey(decreaseKey))
            target = 0.0f;

        throttle = Mathf.MoveTowards(throttle, target, Time.deltaTime * THROTTLE_SPEED);
    }
    
    private void UpdateMouseWheelThrottle()
    {
        throttle += Input.GetAxis("Mouse ScrollWheel");
        throttle = Mathf.Clamp(throttle, 0.0f, 1.0f);
    }    
    
    public void SetPhysicsInput(Vector3 linearInput, Vector3 angularInput)
    {
        appliedLinearForce = Vector3.Scale(linearForce, linearInput);
        appliedAngularForce = Vector3.Scale(angularForce, angularInput);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            asteroidImpacts++;
        }
    }

    private void OnDestroy()
    {
        //death animation
    }

    void launchMissiles()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var pos = _rigidbody.position;
            pos += transform.forward * 5;
            var obj = Instantiate(_missilePrefab, pos, Quaternion.identity);
            launchedMissiles.Add(obj.GetComponent<Missile>());
        }
        
        var temp = new List<Missile>();
        foreach (var missile in launchedMissiles)
        {
            if (missile.destroyed)
                DestroyImmediate(missile.gameObject);
            else
                temp.Add(missile);
        }
        launchedMissiles = temp;
    }
}
