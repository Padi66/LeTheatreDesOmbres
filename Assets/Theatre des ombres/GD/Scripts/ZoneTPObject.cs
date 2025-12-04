using System.Collections;
using UnityEngine;

public class ZoneTPObject : MonoBehaviour
{
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

        ObjectResetter resetter = rb.GetComponent<ObjectResetter>();
        if (resetter == null)
        {
            resetter = rb.gameObject.AddComponent<ObjectResetter>();
        }

        StartCoroutine(ResetObject(rb.transform, rb, resetter));
    }

    private IEnumerator ResetObject(Transform obj, Rigidbody rb, ObjectResetter resetter)
    {
        if (_resetVelocity && !rb.isKinematic)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        bool wasKinematic = rb.isKinematic;
        rb.isKinematic = true;

        obj.position = resetter.InitialPosition;

        if (_resetRotation)
        {
            obj.rotation = resetter.InitialRotation;
        }

        Debug.Log($"{obj.name} téléporté à {resetter.InitialPosition}");

        yield return new WaitForSeconds(_resetDelay);

        rb.isKinematic = wasKinematic;
        
        if (_resetVelocity && !rb.isKinematic)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}