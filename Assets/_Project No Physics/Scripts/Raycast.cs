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
        LayerMask ownMask = LayerMask.GetMask("OwnPlayer");




        if (Physics.Raycast(ray, out hit, 100))
            Debug.DrawLine(ray.origin, hit.point);


        if (Physics.Raycast(ray, out hit, 100, playerMask))
            Debug.Log("Hitting a Player");

        if (Physics.Raycast(ray, out hit, 100, inanimateMask))
            Debug.Log("Hitting inanimate object");

        if (Physics.Raycast(ray, out hit, 100, ownMask))
            Debug.Log("Stop hitting yourself");



    }


}
