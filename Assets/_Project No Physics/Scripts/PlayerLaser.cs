using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    public GameObject laserLineRendererPrefab;  // prefab that holdes line renderer component
    private LineRenderer laserLineRenderer;

    void Start()
    {
        GameObject laserLineRendererObject = Instantiate(laserLineRendererPrefab);
        laserLineRenderer = laserLineRendererObject.GetComponent<LineRenderer>();
        laserLineRenderer.enabled = false;      // initialize line renderer as not visible
    }

    void Update()
    {
        
    }

    public IEnumerator FireLaser(Vector3 origin, Vector3 target)
    {
        laserLineRenderer.SetPosition(0, target);
        laserLineRenderer.SetPosition(1, origin);
        laserLineRenderer.enabled = true;       // show line renderer that represents laser
        yield return new WaitForSeconds(.25f);  // show laser for this many seconds, then hide
        hideLaser();
    }

    private void hideLaser()
    {
        laserLineRenderer.enabled = false;
    }
}
