using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MandController : MonoBehaviour
{
    public string juisteCubeTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(juisteCubeTag))
        {
            Debug.Log("Correct!" + other.tag + "is in " + gameObject.tag);
        }
        else
        {
            Debug.Log("Fout! " + other.tag + "hoort niet in " + gameObject.tag);
        }
    }
}
