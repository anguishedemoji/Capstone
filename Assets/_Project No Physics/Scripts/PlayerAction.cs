using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAction : NetworkBehaviour
{
    public GameObject laserLineRendererPrefab;
    public int laserRange;                  // Range of laser
    public float destroyLaserDelay;         // Time delay before destroying laser
    public Vector3 laserOriginOffset;       // Offset to increase laser visibility

    private Transform cam;                  // local camera transform
    private Quaternion serverCamRotation;   // server camera rotation
    private PlayerInfo playerInfo;

    void Start()
    {
        cam = GetComponentInChildren<Camera>().transform;   // Get position of player camera
        serverCamRotation = cam.rotation;                   // Initialize rotation of camera on server
        playerInfo = GetComponent<PlayerInfo>();            // Get reference to player's info
        laserOriginOffset = new Vector3(0, -.25f, 0);        // Lower origin of raycast so laser is visible
        laserRange = 200;                                   
        destroyLaserDelay = .25f;                           
        Debug.Log("Player: " + netId.Value + ", Health: " + playerInfo.getHealth());
    }

    void Update()
    {
        if (hasAuthority == false)
        {
            cam.rotation = serverCamRotation;    // update camera's rotation to enable proper raycasts
            return;
        }

        // Fire laser if mouse clicked & we have authority over this player
        if (Input.GetMouseButtonDown(0) && hasAuthority == true)
        {
            CmdCreateLaser();  // create laser on server
        }

        CmdUpdateCameraTransform(cam.rotation);
    }

    // Create visible laser beam on server, then determine if player was hit
    [Command]
    void CmdCreateLaser()
    {
        // Get origin of ray based on player heading
        Ray ray = new Ray(cam.position + laserOriginOffset, cam.forward);
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
                localHitPlayer.GetComponent<PlayerInfo>().RpcRegisterHit();
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

    // Async method for creating and destroying visible laser
    private IEnumerator CreateLaser(Vector3 origin, Vector3 target)
    {
        // Instantiate prefab object containing LineRenderer component
        GameObject laserLineRendererObject = Object.Instantiate(laserLineRendererPrefab);
        // Get the new instance's LineRenderer component
        LineRenderer laserLineRenderer = laserLineRendererObject.GetComponent<LineRenderer>();
        laserLineRenderer.SetPosition(0, target);
        laserLineRenderer.SetPosition(1, origin);
        yield return new WaitForSeconds(destroyLaserDelay);  // Show rendered line for this many seconds...
        Destroy(laserLineRendererObject);       // ...then destroy it and its associated game object
    }

    [Command]
    void CmdUpdateCameraTransform(Quaternion rotation)
    {
        serverCamRotation = rotation;
    }
}
