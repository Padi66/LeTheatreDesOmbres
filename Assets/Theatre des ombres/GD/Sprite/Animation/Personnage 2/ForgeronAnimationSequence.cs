using UnityEngine;
using System.Collections;

public class ForgeronAnimationSequence : MonoBehaviour
{
    [Header("Animations à enchaîner")]
    [Tooltip("Nom de la première animation")]
    public string firstAnimationName = "AnimForgeron1";
    
    [Tooltip("Nom de la deuxième animation")]
    public string secondAnimationName = "AnimForgeron2";
    
    [Header("Options")]
    [Tooltip("Délai avant de lancer la deuxième animation")]
    public float delayBeforeSecondAnimation = 0f;
    
    [Tooltip("Lancer automatiquement au démarrage")]
    public bool playOnStart = true;
    
    private Animation animationComponent;
    
    void Start()
    {
        animationComponent = GetComponent<Animation>();
        
        if (animationComponent == null)
        {
            Debug.LogError($"Aucun composant Animation trouvé sur {gameObject.name}!");
            enabled = false;
            return;
        }
        
        if (playOnStart)
        {
            PlaySequence();
        }
    }
    
    public void PlaySequence()
    {
        StartCoroutine(PlayAnimationSequence());
    }
    
    private IEnumerator PlayAnimationSequence()
    {
        animationComponent.Play(firstAnimationName);
        
        yield return new WaitWhile(() => animationComponent.IsPlaying(firstAnimationName));
        
        if (delayBeforeSecondAnimation > 0f)
        {
            yield return new WaitForSeconds(delayBeforeSecondAnimation);
        }
        
        animationComponent.Play(secondAnimationName);
    }
}
