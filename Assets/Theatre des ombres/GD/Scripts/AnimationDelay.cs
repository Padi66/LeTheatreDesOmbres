using System.Collections;
using UnityEngine;

public class AnimationDelay : MonoBehaviour
{
    [Header("Pause Settings")]
    [SerializeField] public float delayBeforeStart = 2f;
    
    [Header("Animation Control")]
    [Tooltip("Si true, désactive tous les composants Animation pendant le délai")]
    [SerializeField] private bool disableLegacyAnimations = true;
    
    [Tooltip("Si true, met la vitesse des Animators à 0 pendant le délai")]
    [SerializeField] private bool pauseAnimators = true;
    
    private Animation[] allAnimations;
    private Animator[] allAnimators;
    private float[] originalAnimatorSpeeds;
    
    private void Awake()
    {
        if (disableLegacyAnimations)
        {
            allAnimations = FindObjectsByType<Animation>(FindObjectsSortMode.None);
            
            foreach (Animation anim in allAnimations)
            {
                anim.enabled = false;
            }
            
            Debug.Log($"[AnimationDelay] {allAnimations.Length} Legacy Animations désactivées");
        }
        
        if (pauseAnimators)
        {
            allAnimators = FindObjectsByType<Animator>(FindObjectsSortMode.None);
            originalAnimatorSpeeds = new float[allAnimators.Length];
            
            for (int i = 0; i < allAnimators.Length; i++)
            {
                originalAnimatorSpeeds[i] = allAnimators[i].speed;
                allAnimators[i].speed = 0f;
            }
            
            Debug.Log($"[AnimationDelay] {allAnimators.Length} Animators mis en pause");
        }
    }
    
    private void Start()
    {
        Time.timeScale = 0f;
        Debug.Log($"[AnimationDelay] Time.timeScale mis à 0 - En attente de ResumeAnimations()");
    }
    
    public void ResumeAnimations()
    {
        Debug.Log("[AnimationDelay] ResumeAnimations() appelé - Réactivation des animations");
        
        Time.timeScale = 1f;
        
        if (disableLegacyAnimations && allAnimations != null)
        {
            foreach (Animation anim in allAnimations)
            {
                if (anim != null)
                {
                    anim.enabled = true;
                }
            }
            
            Debug.Log($"[AnimationDelay] {allAnimations.Length} Legacy Animations réactivées");
        }
        
        if (pauseAnimators && allAnimators != null)
        {
            for (int i = 0; i < allAnimators.Length; i++)
            {
                if (allAnimators[i] != null)
                {
                    allAnimators[i].speed = originalAnimatorSpeeds[i];
                }
            }
            
            Debug.Log($"[AnimationDelay] {allAnimators.Length} Animators relancés");
        }
        
        Debug.Log("[AnimationDelay] Temps restauré, animations actives !");
    }
}

