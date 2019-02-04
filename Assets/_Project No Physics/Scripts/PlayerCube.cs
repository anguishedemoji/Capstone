using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class PlayerCube : NetworkBehaviour
{
    public float moveSpeed;
    public float verticalMoveSpeed = 2.0f;
    public float mouseSpeed;
    private Camera cam;
    private CharacterController controller;

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

        controller = GetComponent<CharacterController>();
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

        float X = Input.GetAxis("Mouse X") * mouseSpeed;
        float Y = Input.GetAxis("Mouse Y") * mouseSpeed;
        cam.transform.Rotate(-Y, 0, 0);

        transform.Rotate(0, X, 0);

        //escape mouse lock
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        // Get player movement input along X and Y axes
        Vector3 direction = new Vector3(-(Input.GetAxis("Horizontal")), 0.0f, -(Input.GetAxis("Vertical")));
        direction = transform.TransformDirection(direction);

        // Vertical movement upwards (+Y direction)
        if (Input.GetKey(KeyCode.Space) || Input.GetAxis("Mouse ScrollWheel") > 0) 
        {
            direction.y = verticalMoveSpeed;
        }

        // Vertical movement downwards (-Y direction)
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            direction.y = -verticalMoveSpeed;
        }
        direction *= moveSpeed;
        controller.Move(direction * Time.deltaTime);

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
