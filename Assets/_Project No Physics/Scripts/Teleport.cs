using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject endpoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if(other.name == "PlayerCube(Clone)")
        {

        other.transform.position = endpoint.transform.position;
            Debug.Log(endpoint.transform.forward);
            float x = endpoint.transform.forward.x;
            float z = endpoint.transform.forward.z;
            other.transform.forward = new Vector3  (x,0,z);
              
        }
    }
}
