using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class PlayerCube : NetworkBehaviour
{
    public float moveSpeed;
    public float mouseSpeed;
    private Camera cam;


    [SyncVar]
    Vector3 serverPosition;

    [SyncVar]
    Quaternion serverplayerRotation;
    Vector3 serverPositionSmoothVelocity;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = GetComponentInChildren<Camera>();
        cam.enabled = false;
    }

    void Update()
    {

        if (hasAuthority == false)
        {
            transform.rotation = serverplayerRotation;
            transform.position = Vector3.SmoothDamp(
                transform.position,
                serverPosition,
                ref serverPositionSmoothVelocity,
                0.25f);

            return;
        }


        cam.enabled = true;

        Vector3 direction = new Vector3(0, 0, 0);

        float X = Input.GetAxis("Mouse X") * mouseSpeed;
        float Y = Input.GetAxis("Mouse Y") * mouseSpeed;
        cam.transform.Rotate(-Y, 0, 0);

        transform.Rotate(0, X, 0);

        //escape mouse lock
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        //left
        if (Input.GetKey(KeyCode.A))
        {

            direction = cam.transform.TransformDirection(-(Time.deltaTime * moveSpeed), 0, 0);


            //transform.Translate(-(Time.deltaTime * moveSpeed), 0, 0, null);
        }

        //right
        if (Input.GetKey(KeyCode.D))
        {
            direction = cam.transform.TransformDirection((Time.deltaTime * moveSpeed), 0, 0);

            //transform.Translate((Time.deltaTime * moveSpeed), 0, 0, null);
        }

        //backwards
        if (Input.GetKey(KeyCode.S))
        {
            direction = cam.transform.TransformDirection(0, 0, -(Time.deltaTime * moveSpeed));

            //transform.Translate(0, 0, -(Time.deltaTime * moveSpeed), null);
        }
        

        //forwards
        if (Input.GetKey(KeyCode.W))
        {
            direction = cam.transform.TransformDirection(0, 0, (Time.deltaTime * moveSpeed));

            //transform.Translate(0, 0, (Time.deltaTime * moveSpeed), null);
        }

        direction.y = 0.0f;

        // Vertical movement upwards (+Y direction)
        if (Input.GetKey(KeyCode.Q)) 
        {
            transform.Translate(0, (Time.deltaTime * moveSpeed), 0, null);
        }

        // Vertical movement downwards (-Y direction)
        if (Input.GetKey(KeyCode.E)) 
        {
            transform.Translate(0, -(Time.deltaTime * moveSpeed), 0, null);
        }



        transform.Translate(direction.x, direction.y, direction.z, null);
        CmdUpdatePosition(transform.position, transform.rotation);
        return;
    }

    [Command]
    void CmdUpdatePosition(Vector3 newPosition, Quaternion rotation)
    {
        //IF Illegal Position update with RpcFixPosition

        serverPosition = newPosition;
        serverplayerRotation = rotation;
    }
}
