using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    public Transform cam;
    Ray ray;

    void Start()
    {
    }

    void Update()
    {
        ray = new Ray(cam.position, cam.forward);

        RaycastHit hit;
        LayerMask playerMask = LayerMask.GetMask("Player");
        LayerMask inanimateMask = LayerMask.GetMask("Inanimate");

        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.DrawLine(ray.origin, hit.point);
        }

        // On player hit
        if (Physics.Raycast(ray, out hit, 100, playerMask))
        {
            if (Input.GetMouseButtonDown(0))
            {
            }
        }

        // On inanimate object hit
        if (Physics.Raycast(ray, out hit, 100, inanimateMask))
        {
            if (Input.GetMouseButtonDown(0))
            {
            }
        }

        // On nothing hit
        if (Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(ray, out hit, 100))
            {
            }
        }
    }
}
