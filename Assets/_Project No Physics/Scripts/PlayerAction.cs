using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAction : NetworkBehaviour
{
    private Transform cam;
    public GameObject laserLineRendererPrefab;

    void Start()
    {
        // Get position of player camera
        cam = GetComponentInChildren<Camera>().transform;
    }

    void Update()
    {
        // Fire laser
        if (Input.GetMouseButtonDown(0) && hasAuthority == true)
        {
            // Get origin of ray based on player heading
            Ray ray = new Ray(cam.position, cam.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                CmdCreateLaser(ray.origin, hit.point);  // create instance on server
            }
        }
    }

    // server-side creation of laser
    [Command]
    void CmdCreateLaser(Vector3 origin, Vector3 target)
    {
        StartCoroutine(FireLaser(origin, target));
        RpcCreateLaser(origin, target);
    }

    [ClientRpc]
    void RpcCreateLaser(Vector3 origin, Vector3 target)
    {
        StartCoroutine(FireLaser(origin, target));
    }

    // Async method for creating and destroying visible laser
    private IEnumerator FireLaser(Vector3 origin, Vector3 target)
    {
        GameObject laserLineRendererObject = Object.Instantiate(laserLineRendererPrefab);
        LineRenderer laserLineRenderer = laserLineRendererObject.GetComponent<LineRenderer>();
        laserLineRenderer.SetPosition(0, target);
        laserLineRenderer.SetPosition(1, origin);
        laserLineRenderer.enabled = true;       // show line renderer that represents laser
        yield return new WaitForSeconds(.25f);  // for this many seconds
        Destroy(laserLineRendererObject);      // ...then destroy it and its associated game object
    }
}
