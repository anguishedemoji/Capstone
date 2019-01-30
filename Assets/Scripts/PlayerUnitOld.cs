using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// A PlayerUnit is a unit controlled by a player
// This could be a character in an FPS, a zergling in a RTS
// Or a scout in a TBS

public class PlayerUnitOld : NetworkBehaviour {

    private Rigidbody rb;
    public float speed;
    public Camera cam;
    public Vector3 offset;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (hasAuthority)
        {
            //cam.enabled = true;
            //offset = cam.transform.position - this.transform.position;
            return;
        }
        cam.enabled = false;

    }

    Vector3 velocity;
    Vector3 bestGuessPosition;
    float ourLatency;  

    // This higher this value, the faster our local position will match the best guess position
    float latencySmoothingFactor = 10;

	void Update () { 

        // How do I verify that I am allowed to mess around with this object?
        if( hasAuthority == false )
        {
            // We aren't the authority for this object, but we still need to update
            bestGuessPosition = bestGuessPosition + ( velocity * Time.deltaTime );

            // Instead of TELEPORTING our position to the best guess's position, we
            // can smoothly lerp to it.
            this.transform.position = Vector3.Lerp( this.transform.position, bestGuessPosition, Time.deltaTime * latencySmoothingFactor);

            return;
        }


        // If we get to here, we are the authoritative owner of this object
       //transform.Translate( velocity * Time.deltaTime );


        if( Input.GetKeyDown(KeyCode.Space) )
        {
            this.transform.Translate( 0, 1, 0 );
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Destroy(gameObject);
        }
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    this.transform.Translate(-1, 0, 0);
        //}

        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    this.transform.Translate(0, 0, 1);
        //}

        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    this.transform.Translate(1, 0, 0);
        //}



    }

    private void FixedUpdate()
    {
        if (hasAuthority == true)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            rb.AddForce(movement * speed);

            velocity = rb.velocity;
            CmdUpdateVelocity(velocity, this.transform.position);
        }
    }

    [Command]
    void CmdUpdateVelocity( Vector3 v, Vector3 p)
    {
        // I am on a server
        this.transform.position = p;
        velocity = v;

        // If we know what our current latency is, we could do something like this:
        //transform.position = p + (v * (ourLatency));


        // Now let the clients know the correct position of this object.
        RpcUpdateVelocity( velocity, this.transform.position);
    }

    [ClientRpc]
    void RpcUpdateVelocity( Vector3 v, Vector3 p )
    {
        // I am on a client

        if( hasAuthority )
        {
            // Let's assume for now that we're just going to ignore the message from the server.
            return;
        }

        velocity = v;
        bestGuessPosition = p + (velocity * (ourLatency));

    }

}
