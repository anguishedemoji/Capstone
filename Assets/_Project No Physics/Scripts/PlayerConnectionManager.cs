using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnectionManager : NetworkBehaviour
{
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

    public void SpawnPlayer()
    {
        if (isServer == false)
        {
            return;
        }
        //Gets called when by Game Manager when a new rounds starts

        GameObject go = Instantiate(PlayerUnitPrefab);

        //Give Authority over Player
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    }
}

