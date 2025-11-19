using System;
using System.Collections;
using UnityEngine;

public class ZoneTPObject : MonoBehaviour
{
    [SerializeField] private GameObject _teleportOrange;
    [SerializeField] private GameObject _teleportViolet;
    [SerializeField] private GameObject _teleportVert;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null) return; 

        if (other.GetComponent<CubeGreen>() != null)
        {
            Debug.Log("Teleport Vert");
            other.transform.position = _teleportVert.transform.position;
        }
        else if (other.GetComponent<CubeOrange>() != null)
        {
            Debug.Log("Teleport Orange");
            other.transform.position = _teleportOrange.transform.position;
        }
        else if (other.GetComponent<CubePurple>() != null)
        {
            Debug.Log("Teleport Violet");
            other.transform.position = _teleportViolet.transform.position;
        }
        else
        {
            return; 
        }
        
        StartCoroutine(DeactivateRigidBody(rb, 2f));
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<CubeGreen>() != null)
        {
            Debug.Log("OnTriggerVert");
        }
        else if (other.gameObject.GetComponent<CubeOrange>() != null)
        {
            Debug.Log("OnTriggerExitOrange");
        }
        else if (other.gameObject.GetComponent<CubePurple>() != null)
        {
            Debug.Log("OnTriggerExitViolet");
        }
    }

    IEnumerator DeactivateRigidBody(Rigidbody rb, float duration)
    {
        rb.isKinematic = true;
        rb.detectCollisions = false;

        yield return new WaitForSeconds(duration);

        rb.isKinematic = false;
        rb.detectCollisions = true;
    }
    }

