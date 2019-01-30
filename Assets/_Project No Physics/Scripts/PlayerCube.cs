using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCube : NetworkBehaviour
{
    public float moveSpeed;
    private Camera cam;

    [SyncVar]
    Vector3 serverPosition;
    Vector3 serverPositionSmoothVelocity;



    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        cam.enabled = false;
    }

    void Update()
    {
    
        if (hasAuthority == false)
        {

            transform.position = Vector3.SmoothDamp(
                transform.position,
                serverPosition,
                ref serverPositionSmoothVelocity,
                0.25f);
            return;
        }


        cam.enabled = true;

        Vector3 direction = new Vector3(0, 0, 0);

        if (Input.GetKey("left")) {
            direction = cam.transform.TransformDirection(-(Time.deltaTime * moveSpeed), 0, 0);


            //transform.Translate(-(Time.deltaTime * moveSpeed), 0, 0, null);
        }

        if (Input.GetKey("right")) 
        {
            direction = cam.transform.TransformDirection((Time.deltaTime * moveSpeed), 0, 0);

            //transform.Translate((Time.deltaTime * moveSpeed), 0, 0, null);
        }

        if (Input.GetKey("down"))
        {
            direction = cam.transform.TransformDirection(0, 0, -(Time.deltaTime * moveSpeed));

            //transform.Translate(0, 0, -(Time.deltaTime * moveSpeed), null);
        }

        if (Input.GetKey("up"))
        {
            direction = cam.transform.TransformDirection(0, 0, (Time.deltaTime * moveSpeed));

            //transform.Translate(0, 0, (Time.deltaTime * moveSpeed), null);
        }

        direction.y = 0.0f;

        transform.Translate(direction.x, direction.y, direction.z, null);
        CmdUpdatePosition(transform.position);
        return;
    }

    [Command]
    void CmdUpdatePosition(Vector3 newPosition)
    {
        //IF Illegal Position update with RpcFixPosition

        serverPosition = newPosition;
    }
}
