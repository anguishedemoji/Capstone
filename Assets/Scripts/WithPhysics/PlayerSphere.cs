using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSphere : NetworkBehaviour
{
    public float moveSpeed = 2.0f;
    private Rigidbody rb;
    private Vector3 currentMovementVector;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (hasAuthority == false)
        {
            rb.AddForce(currentMovementVector);
            return;
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        currentMovementVector = movement * moveSpeed;
        rb.AddForce(currentMovementVector);
    }
}
