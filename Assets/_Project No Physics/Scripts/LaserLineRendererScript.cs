using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLineRendererScript : MonoBehaviour
{
    public float delay = .25f;

    void Start()
    {
        StartCoroutine(DestroySelf());
    }

    private IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(delay);
        Destroy(transform.parent.gameObject);
    }

}
