using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class RayDisabler : MonoBehaviour
{
    [Header("Ray Interactors to disable")]
    [SerializeField] private XRRayInteractor leftRay;
    [SerializeField] private XRRayInteractor rightRay;

    [Header("Settings")]
    [SerializeField] private float disableDuration = 2f;  // How long rays stay disabled

    private void Start()
    {
        StartCoroutine(DisableRaysTemporarily());
    }

    private System.Collections.IEnumerator DisableRaysTemporarily()
    {
        // Disable ray interactors
        if (leftRay != null) leftRay.enabled = false;
        if (rightRay != null) rightRay.enabled = false;

        yield return new WaitForSeconds(disableDuration);

        // Re-enable after delay
        if (leftRay != null) leftRay.enabled = true;
        if (rightRay != null) rightRay.enabled = true;
    }
}