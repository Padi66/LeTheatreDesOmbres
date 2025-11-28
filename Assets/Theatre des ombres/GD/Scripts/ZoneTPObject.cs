using System.Collections;
using UnityEngine;

public class ZoneTPObject : MonoBehaviour
{
    [Header("Configuration")] [SerializeField]
    private float _resetDelay = 0.1f;

    [SerializeField] private bool _resetRotation = true;
    [SerializeField] private bool _resetVelocity = true;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[ZoneTP] Trigger entré par: {other.name}");
    
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.Log($"[ZoneTP] {other.name} n'a pas de Rigidbody");
            return;
        }

        ObjectResetter resetter = other.GetComponent<ObjectResetter>();
    
        if (resetter == null)
        {
            Debug.Log($"[ZoneTP] {other.name} n'a pas de ObjectResetter");
            return;
        }

        if (resetter != null &&
            (other.GetComponent<CubeGreen>() != null ||
             other.GetComponent<CubeOrange>() != null ||
             other.GetComponent<CubePurple>() != null ||
             other.GetComponent<Sword>() != null ||
             other.GetComponent<Shield>() != null))


        {
            StartCoroutine(ResetObject(other.transform, rb, resetter));
        }
    }

    private IEnumerator ResetObject(Transform obj, Rigidbody rb, ObjectResetter resetter)
    {
        rb.isKinematic = true;

        if (_resetVelocity)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        obj.position = resetter.InitialPosition;

        if (_resetRotation)
        {
            obj.rotation = resetter.InitialRotation;
        }

        Debug.Log($"{obj.name} téléporté à sa position initiale");

        yield return new WaitForSeconds(_resetDelay);

        rb.isKinematic = false;
    }
}
