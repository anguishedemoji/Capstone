using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Raycast : MonoBehaviour
{
    public Transform cam;
    private PlayerLaser playerLaser;
    Ray ray;

    void Start()
    {
        playerLaser = GetComponent<PlayerLaser>();
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

        if (Physics.Raycast(ray, out hit, 100, playerMask))
        {
            if (Input.GetMouseButtonDown(0))
            {
              // Debug.Log("Shooting Player");
                playerLaser.FireLaser(ray.origin, hit.point);
            }
        }

        if (Physics.Raycast(ray, out hit, 100, inanimateMask))
        {
<<<<<<< HEAD
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Shooting Object");
                StartCoroutine(playerLaser.FireLaser(ray.origin, hit.point));
=======
            //reticle.color = Color.red;
           // Debug.Log("Hitting inanimate object");
           // Debug.Log(hit);
            if (Input.GetMouseButtonDown(0))
            {
               // Debug.Log("Shooting Object");
                playerLaser.FireLaser(ray.origin, hit.point);
>>>>>>> newMerge
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
           // Debug.Log("Shooting Nothing");
            if(Physics.Raycast(ray, out hit, 100))
            {
                StartCoroutine(playerLaser.FireLaser(ray.origin, hit.point));
            }
        }
    }
}
