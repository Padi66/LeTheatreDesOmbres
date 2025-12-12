using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ZoneTPObject : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float _resetDelay = 0.1f;
    [SerializeField] private bool _resetRotation = true;
    [SerializeField] private bool _resetVelocity = true;
    [SerializeField] private GameObject _particlePrefab;
    /*private ParticleSystem _particleSystem;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _teleportSound;*/


    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = other.GetComponentInParent<Rigidbody>();
        }
        /*other.gameObject.AddComponent<AudioSource>();
        _audioSource = other.gameObject.GetComponent<AudioSource>();
        _audioSource.clip = _teleportSound;
        _audioSource.playOnAwake = false;
        GameObject particleInstance = Instantiate(_particlePrefab, other.transform);
        particleInstance.transform.localPosition = Vector3.zero;
        particleInstance.transform.localRotation = Quaternion.identity;
        _particleSystem = particleInstance.GetComponent<ParticleSystem>();*/



        
        
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

        rb.isKinematic = true;

        obj.position = resetter.InitialPosition;

        if (_resetRotation)
        {
            obj.rotation = resetter.InitialRotation;
        }

        Debug.Log($"{obj.name} téléporté à {resetter.InitialPosition}");

        yield return new WaitForSeconds(_resetDelay);

        if (grabInteractable == null)
        {
            rb.isKinematic = false;
        }
        else
        {
            rb.isKinematic = false;
        }
    
        /*_audioSource.Play();
        _particleSystem.Play();*/
    
        if (_resetVelocity && !rb.isKinematic)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }


    // ORIGINAL ResetObject method before modification:
    /*
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
    */
}
