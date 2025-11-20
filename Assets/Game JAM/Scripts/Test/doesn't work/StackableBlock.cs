using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(Rigidbody))]
public class StackableBlock : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private FixedJoint joint;
    private Transform socketParentTransform;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
            grabInteractable.selectExited.RemoveListener(OnSelectExited);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactorObject is XRSocketInteractor socketInteractor)
        {
            socketParentTransform = socketInteractor.transform.parent;
            
            if (socketParentTransform != null && socketParentTransform.TryGetComponent<Rigidbody>(out Rigidbody parentRb))
            {
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                
                joint = gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = parentRb;
                joint.enableCollision = false;
                joint.breakForce = Mathf.Infinity;
                joint.breakTorque = Mathf.Infinity;
                
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.mass = 1f;
                rb.linearDamping = 0;
                rb.angularDamping = 0.05f;
            }
            else
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (args.interactorObject is XRSocketInteractor)
        {
            if (joint != null)
            {
                Destroy(joint);
                joint = null;
            }
            
            socketParentTransform = null;
            
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
        }
    }
}
