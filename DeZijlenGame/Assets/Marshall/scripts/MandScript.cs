using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MandController : MonoBehaviour
{
    public string juisteCubeTag;

    public Transform redTeleportLocation;
    public Transform blueTeleportLocation;
    public Transform greenTeleportLocation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(juisteCubeTag))
        {
            Debug.Log("Correct!" + other.tag + "is in " + gameObject.tag);
        }
        else
        {
            Debug.Log("Fout! " + other.tag + "hoort niet in " + gameObject.tag);
            Transform teleportLocation = GetTeleportLocation(other.tag);

            if (teleportLocation != null)
            {
                other.transform.position = teleportLocation.position;
            }
        }
    }

    private Transform GetTeleportLocation(string tag)
    {
        switch (tag)
        {
            case "RedCube":
                return redTeleportLocation;
            case "BlueCube":
                return blueTeleportLocation;
            case "GreenCube":
                return greenTeleportLocation;
            default:
                return null;
        }
    }
}
