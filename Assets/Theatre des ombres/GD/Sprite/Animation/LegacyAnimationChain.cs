using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LegacyAnimationChain : MonoBehaviour
{
    [Header("Main Animation")]
    [Tooltip("Nom de l'animation principale qui déclenche les autres")]
    public string mainAnimationName = "Slice1";
    
    [Header("Auto Detection")]
    [Tooltip("Chercher automatiquement tous les objets avec Animation dans la scène")]
    public bool autoDetectAnimations = true;
    
    [Header("Manual Setup (si autoDetectAnimations = false)")]
    public Animation mainAnimation;
    public Animation[] animationsToTrigger;
    
    [Header("Settings")]
    public float delay = 0f;
    
    private Animation detectedMainAnimation;
    private List<Animation> detectedAnimations = new List<Animation>();
    
    void Start()
    {
        if (autoDetectAnimations)
        {
            FindAllAnimations();
        }
        else
        {
            detectedMainAnimation = mainAnimation;
            detectedAnimations.AddRange(animationsToTrigger);
        }
        
        if (detectedMainAnimation == null)
        {
            Debug.LogError("Aucune animation principale trouvée !");
            enabled = false;
            return;
        }
        
        StartCoroutine(WatchMainAnimation());
    }
    
    void FindAllAnimations()
    {
        Animation[] allAnimations = FindObjectsByType<Animation>(FindObjectsSortMode.None);
        
        foreach (Animation anim in allAnimations)
        {
            AnimationClip clip = anim.GetClip(mainAnimationName);
            
            if (clip != null)
            {
                detectedMainAnimation = anim;
                Debug.Log($"Animation principale trouvée sur: {anim.gameObject.name}");
            }
            else
            {
                detectedAnimations.Add(anim);
            }
        }
        
        Debug.Log($"Détecté {detectedAnimations.Count} animations à déclencher");
    }
    
    IEnumerator WatchMainAnimation()
    {
        while (true)
        {
            yield return new WaitUntil(() => detectedMainAnimation.IsPlaying(mainAnimationName));
            
            Debug.Log($"{mainAnimationName} commence à jouer...");
            
            yield return new WaitWhile(() => detectedMainAnimation.IsPlaying(mainAnimationName));
            
            Debug.Log($"{mainAnimationName} terminée ! Déclenchement des autres animations...");
            
            if (delay > 0f)
            {
                yield return new WaitForSeconds(delay);
            }
            
            TriggerAnimations();
        }
    }
    
    void TriggerAnimations()
    {
        int triggered = 0;
        foreach (Animation anim in detectedAnimations)
        {
            if (anim != null)
            {
                anim.Play();
                triggered++;
                Debug.Log($"Animation déclenchée sur: {anim.gameObject.name}");
            }
        }
        
        Debug.Log($"{triggered} animations déclenchées au total");
    }
}
