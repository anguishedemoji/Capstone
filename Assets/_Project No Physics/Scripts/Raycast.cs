using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Raycast : MonoBehaviour
{
    public Transform cam;
    private GameObject canvas;
    private RawImage reticle;
    private PlayerLaser playerLaser;
    Ray ray;

    void Start()
    {
        //canvas = GameObject.Find("reticle");
        //reticle = canvas.GetComponent<RawImage>();
        //Debug.Log(canvas);
        playerLaser = GetComponent<PlayerLaser>();

    }

    void Update()
    {


        ray = new Ray(cam.position, cam.forward);

        RaycastHit hit;
        LayerMask playerMask = LayerMask.GetMask("Player");
        LayerMask inanimateMask = LayerMask.GetMask("Inanimate");

        if (Physics.Raycast(ray, out hit, 100))
            Debug.DrawLine(ray.origin, hit.point);

        if (Physics.Raycast(ray, out hit, 100, playerMask))
        {
            reticle.color = Color.cyan;


            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Shooting Player");
                playerLaser.FireLaser(ray.origin, hit.point);
            }

        }

        if (Physics.Raycast(ray, out hit, 100, inanimateMask))
        {
            //reticle.color = Color.red;
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Shooting Object");
                StartCoroutine(playerLaser.FireLaser(ray.origin, hit.point));
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Shooting Nothing");
            if(Physics.Raycast(ray, out hit, 100))
            {
                StartCoroutine(playerLaser.FireLaser(ray.origin, hit.point));
            }
        }


    }
}
