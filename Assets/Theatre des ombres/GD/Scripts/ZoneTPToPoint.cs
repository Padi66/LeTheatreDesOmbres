using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ZoneTPToPoint : MonoBehaviour
{
    [Header("Teleport Target")]
    [SerializeField] private Transform _targetPoint;
    
    [Header("Filter")]
    [SerializeField] private List<string> _allowedTags = new List<string> { "Cube", "Triangle", "Pyramide" };
    
    [Header("Configuration")]
    [SerializeField] private float _resetDelay = 0.1f;
    [SerializeField] private bool _resetRotation = true;
    [SerializeField] private bool _resetVelocity = true;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = other.GetComponentInParent<Rigidbody>();
        }
        
        if (rb == null) return;

        if (!IsAllowedTag(rb.tag))
        {
            return;
        }

        if (_targetPoint == null)
        {
            Debug.LogWarning($"Target Point not set on {gameObject.name}");
            return;
        }

        StartCoroutine(TeleportObject(rb.transform, rb));
    }

    private bool IsAllowedTag(string objectTag)
    {
        if (_allowedTags == null || _allowedTags.Count == 0)
        {
            return true;
        }

        return _allowedTags.Contains(objectTag);
    }

    private IEnumerator TeleportObject(Transform obj, Rigidbody rb)
    {
        XRGrabInteractable grabInteractable = rb.GetComponent<XRGrabInteractable>();
        
        if (grabInteractable != null && grabInteractable.isSelected)
        {
            grabInteractable.interactionManager.CancelInteractableSelection((IXRSelectInteractable)grabInteractable);
        }
        
        if (_resetVelocity && !rb.isKinematic)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        bool wasKinematic = rb.isKinematic;
        rb.isKinematic = true;

        obj.position = _targetPoint.position;

        if (_resetRotation)
        {
            obj.rotation = _targetPoint.rotation;
        }

        Debug.Log($"{obj.name} téléporté à {_targetPoint.position}");

        yield return new WaitForSeconds(_resetDelay);

        rb.isKinematic = wasKinematic;
        
        if (_resetVelocity && !rb.isKinematic)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
