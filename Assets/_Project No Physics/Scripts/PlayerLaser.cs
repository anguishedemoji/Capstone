using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    public GameObject laserLineRendererPrefab;
    private LineRenderer laserLineRenderer;

    void Start()
    {
        GameObject laserLineRendererObject = Instantiate(laserLineRendererPrefab);
        laserLineRenderer = laserLineRendererObject.GetComponent<LineRenderer>();
        laserLineRenderer.enabled = false;
    }

    void Update()
    {
        
    }

    public void FireLaser(Vector3 origin, Vector3 target)
    {
        laserLineRenderer.SetPosition(0, target);
        laserLineRenderer.SetPosition(1, origin);
        laserLineRenderer.enabled = true;
    }

    private void hideLaser()
    {
        laserLineRenderer.enabled = false;
    }
}
