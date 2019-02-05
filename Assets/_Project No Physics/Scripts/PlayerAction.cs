using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAction : NetworkBehaviour
{
    public GameObject laserLineRendererPrefab;
    public int laserRange;

    private Transform cam;
    private PlayerInfo playerInfo;

    void Start()
    {
        cam = GetComponentInChildren<Camera>().transform;   // Get position of player camera
        playerInfo = GetComponent<PlayerInfo>();            // Get reference to player's info
        laserRange = 200;                                   // Initialize range of laser
        Debug.Log("Player: " + netId.Value + ", Health: " + playerInfo.playerHealth);
    }

    void Update()
    {
        // Fire laser if mouse clicked & we have authority over this player
        if (Input.GetMouseButtonDown(0) && hasAuthority == true)
        {
            CmdCreateLaser();  // create laser on server
        }
    }

    // Create visible laser beam on server, then determine if player was hit
    [Command]
    void CmdCreateLaser()
    {
        // Get origin of ray based on player heading
        Ray ray = new Ray(cam.position, cam.forward);
        RaycastHit hit;
        // if raycast hits something
        if (Physics.Raycast(ray, out hit, laserRange))
        {
            RpcCreateLaser(ray.origin, hit.point);              // create visible lasers on clients
            if (hit.transform.gameObject.name == "CapGuy")      // if ray hits a player
            {
                // Get netId from CapGuy model's parent gameObject
                NetworkIdentity hitPlayerIdentity = hit.transform.parent.gameObject.GetComponent<NetworkIdentity>();
                Debug.Log("Network Id: " + hitPlayerIdentity.netId);
                PlayerCube localHitPlayer = NetworkServer.FindLocalObject(hitPlayerIdentity.netId).GetComponent<PlayerCube>();
                Debug.Log("localHitPlayer: " + localHitPlayer);
                localHitPlayer.GetComponent<PlayerAction>().RpcRegisterHit();
            }
        }
        // If raycast hits nothing
        else
        {
            RpcCreateLaser(ray.origin, cam.forward * laserRange);
        }
    }

    // Create visible laser beam on client
    [ClientRpc]
    void RpcCreateLaser(Vector3 origin, Vector3 point)
    {
        StartCoroutine(CreateLaser(origin, point));
    }

    // Register hit on appropriate player
    [ClientRpc]
    public void RpcRegisterHit()
    {
        playerInfo.playerHealth -= 5;
        Debug.Log("Player Health Decremented. Health: " + playerInfo.playerHealth);
    }

    // Async method for creating and destroying visible laser
    private IEnumerator CreateLaser(Vector3 origin, Vector3 target)
    {
        // Instantiate prefab object containing LineRenderer component
        GameObject laserLineRendererObject = Object.Instantiate(laserLineRendererPrefab);
        // Get the new instance's LineRenderer component
        LineRenderer laserLineRenderer = laserLineRendererObject.GetComponent<LineRenderer>();
        laserLineRenderer.SetPosition(0, target);
        laserLineRenderer.SetPosition(1, origin);
        yield return new WaitForSeconds(.25f);  // Show rendered line for this many seconds...
        Destroy(laserLineRendererObject);       // ...then destroy it and its associated game object
    }
}
