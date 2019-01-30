using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnectionManager : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // FOR NOW spawn immediately
        // WHen dead respawn in a couple seconds
        if (isServer == true)
        {
            SpawnPlayer();
        }

    }

    public GameObject PlayerUnitPrefab;

    // Update is called once per frame
    void Update()
    {


    }

    public void SpawnPlayer()
    {
        if (isServer == false)
        {
            Debug.LogError("Why was it called from client!!!");
            return;
        }
        //Gets called when by Game Manager when a new rounds starts
        //TODO: maybe pick customize, colors, name etc

        GameObject go = Instantiate(PlayerUnitPrefab);

        // The is to tell everyone else to spawn it from the Server
        //NetworkServer.Spawn(go);

        //Give Authority over Player
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    }


}

