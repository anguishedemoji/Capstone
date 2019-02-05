using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Note: Start runs beofre anyone connects to any server
    }

    //If this changes on server should update on all clients
    [SyncVar]
    public float TimeLeft = 180;

    // Update is called once per frame
    void Update()
    {
        if(isServer == false)
        {
            return;
        }
        //FOR NOW  -- we just reset the map/players every 3 minutes
        TimeLeft -= Time.deltaTime;
        if(TimeLeft <= 0)
        {
            //
        }
    }
}
