using UnityEngine;
using Unity.XR.CoreUtils;

public class CameraHeightNormalizer : MonoBehaviour
{
    [Header("Height Settings")]
    [Tooltip("Hauteur cible pour tous les joueurs")]
    public float targetHeight = 1.7f;

    [Tooltip("Hauteur minimale pour éviter de passer sous le sol")]
    public float minHeight = 0.5f;

    private XROrigin xrOrigin;
    private Camera xrCamera;
    private Transform cameraOffset;

    void Start()
    {
        xrOrigin = GetComponent<XROrigin>();
        if (xrOrigin != null)
        {
            xrCamera = xrOrigin.Camera;
            cameraOffset = xrOrigin.CameraFloorOffsetObject.transform;
        }
    }

    void LateUpdate()
    {
        if (xrCamera == null || cameraOffset == null) return;

        ApplyHeightCorrection();
    }

    void ApplyHeightCorrection()
    {
        float currentCameraLocalHeight = xrCamera.transform.localPosition.y;

        float desiredOffset = targetHeight - currentCameraLocalHeight;

        desiredOffset = Mathf.Max(desiredOffset, minHeight);

        Vector3 offsetPosition = cameraOffset.localPosition;
        offsetPosition.y = desiredOffset;
        cameraOffset.localPosition = offsetPosition;
    }
}
