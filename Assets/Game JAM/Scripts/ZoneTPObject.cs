using System;
using UnityEngine;

public class ZoneTPObject : MonoBehaviour
{
    [SerializeField] private GameObject _teleportOrange;
    [SerializeField] private GameObject _teleportViolet;
    [SerializeField] private GameObject _teleportVert;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CubeVert>() != null)
        {
            Debug.Log("OnTriggerExitVert");
            other.transform.position = _teleportVert.transform.position;
            
        }
        else if (other.gameObject.GetComponent<CubeOrange>() != null)
        {
            Debug.Log("OnTriggerExitVert");
            other.transform.position = _teleportOrange.transform.position;
        }
        else if (other.gameObject.GetComponent<CubeViolet>() != null)
        {
            other.transform.position = _teleportViolet.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<CubeVert>() != null)
        {
            Debug.Log("OnTriggerVert");
        }
        else if (other.gameObject.GetComponent<CubeOrange>() != null)
        {
            Debug.Log("OnTriggerExitOrange");
        }
        else if (other.gameObject.GetComponent<CubeViolet>() != null)
        {
            Debug.Log("OnTriggerExitViolet");
        }
    }   

}
