using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(Rigidbody))]
public class FusableBlock : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    
    private bool isFused = false;
    private FusableBlock rootBlock;
    private ConfigurableJoint joint;
    private BoxCollider blockCollider;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        blockCollider = GetComponent<BoxCollider>();

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
            FuseWithBlock(socketInteractor);
        }
        else if (isFused)
        {
            ForceUnfuse();
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (args.interactorObject is XRSocketInteractor && isFused)
        {
            Unfuse();
        }
    }

    private void FuseWithBlock(XRSocketInteractor socketInteractor)
    {
        Transform socketParent = socketInteractor.transform.parent;

        if (socketParent != null && socketParent.TryGetComponent<FusableBlock>(out FusableBlock parent))
        {
            rootBlock = parent.GetRootBlock();
            Rigidbody rootRb = rootBlock.rb;
            
            if (joint == null)
            {
                joint = gameObject.AddComponent<ConfigurableJoint>();
            }
            
            joint.connectedBody = rootRb;
            joint.autoConfigureConnectedAnchor = false;
            
            Vector3 localAnchor = transform.InverseTransformPoint(socketInteractor.attachTransform.position);
            joint.anchor = localAnchor;
            joint.connectedAnchor = rootRb.transform.InverseTransformPoint(socketInteractor.attachTransform.position);
            
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Locked;
            
            joint.enableCollision = false;
            joint.enablePreprocessing = false;
            
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            
            if (blockCollider != null)
            {
                Physics.IgnoreCollision(blockCollider, rootBlock.blockCollider, true);
            }
            
            isFused = true;
            
            rootBlock.UpdateTowerMass();
        }
    }

    private void Unfuse()
    {
        if (isFused && rootBlock != null)
        {
            if (joint != null)
            {
                Destroy(joint);
                joint = null;
            }
            
            if (blockCollider != null && rootBlock.blockCollider != null)
            {
                Physics.IgnoreCollision(blockCollider, rootBlock.blockCollider, false);
            }
            
            rb.useGravity = true;
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            
            FusableBlock oldRoot = rootBlock;
            rootBlock = null;
            isFused = false;
            
            oldRoot.UpdateTowerMass();
        }
    }

    private FusableBlock GetRootBlock()
    {
        if (isFused && rootBlock != null)
        {
            return rootBlock.GetRootBlock();
        }
        return this;
    }

    private void UpdateTowerMass()
    {
        if (!isFused)
        {
            int blockCount = 1;
            FusableBlock[] children = GetComponentsInChildren<FusableBlock>(true);
            
            foreach (FusableBlock child in children)
            {
                if (child != this && child.isFused && child.rootBlock == this)
                {
                    blockCount++;
                }
            }
            
            rb.mass = Mathf.Max(1f, blockCount * 1f);
        }
    }

    public void ForceUnfuse()
    {
        if (isFused)
        {
            XRSocketInteractor[] sockets = FindObjectsByType<XRSocketInteractor>(FindObjectsSortMode.None);
            foreach (XRSocketInteractor socket in sockets)
            {
                if (socket.hasSelection && socket.interactablesSelected.Contains(grabInteractable))
                {
                    socket.interactionManager.SelectExit((IXRSelectInteractor)socket, (IXRSelectInteractable)grabInteractable);

                    break;
                }
            }
        }
    }
}
