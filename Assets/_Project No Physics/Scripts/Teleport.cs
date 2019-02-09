using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject endpoint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "PlayerGameObject(Clone)")
        {
            float x = endpoint.transform.forward.x;
            float z = endpoint.transform.forward.z;
            Vector3 newForward = new Vector3(x, 0, z);
            other.transform.position = endpoint.transform.position;
            other.transform.forward = newForward;
        }
    }
}
