using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCube : NetworkBehaviour
{
    public float moveSpeed = 2.0f;

    void Start()
    {
        
    }

    void Update()
    {
        if (hasAuthority == false)
        {
            transform.position = this.transform.position;
            transform.rotation = this.transform.rotation;
            return;
        }

        if (Input.GetKey("left")) {
            transform.Translate(-(Time.deltaTime * moveSpeed), 0, 0, null);
        }

        if (Input.GetKey("right")) {
            transform.Translate((Time.deltaTime * moveSpeed), 0, 0, null);
        }

        if (Input.GetKey("down"))
        {
            transform.Translate(0, 0, -(Time.deltaTime * moveSpeed), null);
        }

        if (Input.GetKey("up"))
        {
            transform.Translate(0, 0, (Time.deltaTime * moveSpeed), null);
        }

    }
}
