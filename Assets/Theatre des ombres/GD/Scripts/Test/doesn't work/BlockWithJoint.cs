using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class BlockWithJoint : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private FixedJoint joint;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        grabInteractable.selectEntered.AddListener(OnSocketAttach);
        grabInteractable.selectExited.AddListener(OnSocketDetach);
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnSocketAttach);
        grabInteractable.selectExited.RemoveListener(OnSocketDetach);
    }

    private void OnSocketAttach(SelectEnterEventArgs args)
    {
        if (args.interactorObject is XRSocketInteractor socketInteractor)
        {
            Transform socketParent = socketInteractor.transform.parent;
            if (socketParent != null && socketParent.TryGetComponent<Rigidbody>(out Rigidbody parentRb))
            {
                joint = gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = parentRb;
                
                rb.isKinematic = false;
                rb.useGravity = true;
            }
        }
    }

    private void OnSocketDetach(SelectExitEventArgs args)
    {
        if (joint != null)
        {
            Destroy(joint);
            joint = null;
        }
    }
}