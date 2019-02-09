using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LocalCanvas : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)
        {
            Debug.Log("I am local");
        }
        GetComponentInChildren<Canvas>().enabled = true;
    }
}
