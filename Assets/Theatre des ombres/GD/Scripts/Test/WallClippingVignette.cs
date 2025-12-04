using UnityEngine;

public class WallClippingVignette : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 0.15f;
    [SerializeField] private LayerMask wallLayers = ~0;
    
    [Header("Fade Settings")]
    [SerializeField] private float fadeSpeed = 5f;
    [SerializeField] private Color vignetteColor = Color.black;
    [SerializeField] private float quadDistance = 0.1f;
    [SerializeField] private float quadSize = 1f;
    
    private GameObject vignetteQuad;
    private Material vignetteMaterial;
    private float currentAlpha = 0f;
    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = transform;
        CreateVignetteQuad();
    }

    private void CreateVignetteQuad()
    {
        vignetteQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        vignetteQuad.name = "WallClippingVignetteQuad";
        vignetteQuad.transform.SetParent(cameraTransform);
        vignetteQuad.transform.localPosition = new Vector3(0f, 0f, quadDistance);
        vignetteQuad.transform.localRotation = Quaternion.identity;
        vignetteQuad.transform.localScale = new Vector3(quadSize, quadSize, 1f);
        
        Destroy(vignetteQuad.GetComponent<Collider>());
        
        vignetteMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        vignetteMaterial.SetFloat("_Surface", 1);
        vignetteMaterial.SetFloat("_Blend", 0);
        vignetteMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        vignetteMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        vignetteMaterial.SetInt("_ZWrite", 0);
        vignetteMaterial.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        vignetteMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        vignetteMaterial.renderQueue = 3000;
        
        vignetteMaterial.SetColor("_BaseColor", new Color(vignetteColor.r, vignetteColor.g, vignetteColor.b, 0f));
        
        MeshRenderer renderer = vignetteQuad.GetComponent<MeshRenderer>();
        renderer.material = vignetteMaterial;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        
        vignetteQuad.layer = LayerMask.NameToLayer("UI");
        
        Debug.Log("[WallClippingVignette] Vignette quad created successfully");
    }

    private void Update()
    {
        bool isInsideWall = CheckIfInsideWall();
        
        float targetAlpha = isInsideWall ? 1f : 0f;
        currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);
        
        if (vignetteMaterial != null)
        {
            vignetteMaterial.SetColor("_BaseColor", new Color(vignetteColor.r, vignetteColor.g, vignetteColor.b, currentAlpha));
        }
    }

    private bool CheckIfInsideWall()
    {
        Collider[] colliders = Physics.OverlapSphere(cameraTransform.position, detectionRadius, wallLayers);
        
        foreach (Collider collider in colliders)
        {
            if (!collider.isTrigger)
            {
                return true;
            }
        }
        
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (cameraTransform == null)
            cameraTransform = transform;
            
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(cameraTransform.position, detectionRadius);
    }

    private void OnDestroy()
    {
        if (vignetteMaterial != null)
        {
            Destroy(vignetteMaterial);
        }
        if (vignetteQuad != null)
        {
            Destroy(vignetteQuad);
        }
    }
}
