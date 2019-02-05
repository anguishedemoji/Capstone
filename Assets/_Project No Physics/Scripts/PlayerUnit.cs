using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerUnit : NetworkBehaviour
{
    //Where the server things position for object is 
    [SyncVar]
    Vector3 serverPosition;
    Vector3 serverPositionSmoothVelocity;

    float Speed = 5;

    //public float speedBody;
    //private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            //can maybe update damage instead of on local object
        }

        if (hasAuthority)
        {
            AuthorityUpdate();
        }


        if (hasAuthority == false)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                serverPosition,
                ref serverPositionSmoothVelocity,
                0.25f * Time.deltaTime);

        }
    }

    void AuthorityUpdate()
    {
        //Debug.Log("AuthorityUpdate");
        float moveHorizontal = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
        float moveVertical = Input.GetAxis("Vertical") * Speed * Time.deltaTime;

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        transform.Translate(movement);

        CmdUpdatePosition(transform.position);
    }

    [Command]
    void CmdUpdatePosition(Vector3 newPosition)
    {
        //IF Illegal Position update with RpcFixPosition

        serverPosition = newPosition;
    }

    [ClientRpc]
    void RpcFixPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }


}
