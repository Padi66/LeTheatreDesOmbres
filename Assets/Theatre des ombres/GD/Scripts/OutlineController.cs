using UnityEngine;

public class OutlineController : MonoBehaviour
{
    private const string OUTLINE_LAYER_NAME = "Outline";
    private const string DEFAULT_LAYER_NAME = "Default";
    
    private int outlineLayer;
    private int defaultLayer;
    
    private void Awake()
    {
        outlineLayer = LayerMask.NameToLayer(OUTLINE_LAYER_NAME);
        defaultLayer = LayerMask.NameToLayer(DEFAULT_LAYER_NAME);
        
        if (outlineLayer == -1)
        {
            Debug.LogError($"Layer '{OUTLINE_LAYER_NAME}' n'existe pas!");
        }
    }
    
    public void EnableOutline()
    {
        gameObject.layer = outlineLayer;
        Debug.Log($"Outline activé sur {gameObject.name}");
    }
    
    public void DisableOutline()
    {
        gameObject.layer = defaultLayer;
        Debug.Log($"Outline désactivé sur {gameObject.name}");
    }
    
    public void ToggleOutline()
    {
        if (gameObject.layer == outlineLayer)
        {
            DisableOutline();
        }
        else
        {
            EnableOutline();
        }
    }
}