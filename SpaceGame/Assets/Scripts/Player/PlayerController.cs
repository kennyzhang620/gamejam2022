using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody _rigidbody;
    private Quaternion constRotation;
    private Vector2 lastMousePos;
    private Camera playerCam;

    public float speed = 5;
    public float sprintSpeed = 20;
    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        playerCam = GetComponentInChildren<Camera>();
        constRotation =  _rigidbody.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        //detect mouse movement
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        float rotMultiplier = 0.15f;
        float sensitivity = 4;
        if (mousePos.Equals(lastMousePos))
        {
            rotMultiplier = 0.01f;
        }

        if (Input.GetAxis("Mouse X") > 0)
        {
            _rigidbody.transform.Rotate(0, 1*sensitivity, 0);
        }

        if (Input.GetAxis("Mouse X") < 0)
        {
            _rigidbody.transform.Rotate(0, -1*sensitivity, 0);
        }

        //Movement Update
        float horMul = Input.GetAxisRaw("Horizontal")*speed;
        float verMul = Input.GetAxisRaw("Vertical")*speed;

        if (_rigidbody.velocity.magnitude <= speed)
        {
            _rigidbody.AddRelativeForce(0, 0, 10 * verMul);

            _rigidbody.AddRelativeForce(10 * horMul, 0, 0);
        }
        else if (Input.GetAxis("Fire1") != 0 && _rigidbody.velocity.magnitude <= sprintSpeed)
        {
            _rigidbody.AddRelativeForce(0, 0, 10 * verMul);

            _rigidbody.AddRelativeForce(10 * horMul, 0, 0);
        }
        /*
                Vector3 newVel = _rigidbody.velocity;
                newVel.x = verMul;
                newVel.z = horMul;
                Vector3 rotatedVel = Vector3.RotateTowards(newVel, transform.forward, 10000f, 10000f);
                rotatedVel = rotatedVel.normalized * newVel.magnitude;
                _rigidbody.velocity = rotatedVel;

                _rigidbody.angularVelocity.Set(0, 0, 0);


                //Camera Direction update based on mouse
                Vector3 point = playerCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
                point.y = transform.position.y;
                Quaternion rot = transform.rotation;
                transform.LookAt(point, Vector3.up);
                Quaternion newRot = Quaternion.Slerp(rot, transform.rotation, rotMultiplier);
                transform.rotation = Quaternion.Euler(0, newRot.eulerAngles.y, 0);
                lastMousePos = mousePos;
                // constRotation = transform.rotation;

        */
    }
}
