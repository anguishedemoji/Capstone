using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ThirdPersonClient : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // How do I verify that I am allowed to mess around with this object?
        if (hasAuthority == false)
        {
            // We aren't the authority for this object, but we still need to update
            bestGuessPosition = bestGuessPosition + (velocity * Time.deltaTime);

            // Instead of TELEPORTING our position to the best guess's position, we
            // can smoothly lerp to it.
            this.transform.position = Vector3.Lerp(this.transform.position, bestGuessPosition, Time.deltaTime * latencySmoothingFactor);

            return;
        }

        this.transform.Translate(velocity * Time.deltaTime);
        CmdUpdateVelocity(velocity, this.transform.position);

        // If we get to here, we are the authoritative owner of this object
        //transform.Translate( velocity * Time.deltaTime );

    }

    Vector3 velocity;
    Vector3 bestGuessPosition;
    float ourLatency;
    float latencySmoothingFactor = 10;

    [Command]
    void CmdUpdateVelocity(Vector3 v, Vector3 p)
    {
        // I am on a server
        this.transform.position = p;
        velocity = v;

        RpcUpdateVelocity(velocity, this.transform.position);
    }

    [ClientRpc]
    void RpcUpdateVelocity(Vector3 v, Vector3 p)
    {
        // I am on a client

        if (hasAuthority)
        {
            // Let's assume for now that we're just going to ignore the message from the server.
            return;
        }

        velocity = v;
        bestGuessPosition = p + (velocity * (ourLatency));

    }
}
