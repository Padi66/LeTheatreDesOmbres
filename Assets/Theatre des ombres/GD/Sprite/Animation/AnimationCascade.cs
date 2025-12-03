using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationCascade : MonoBehaviour
{
    [System.Serializable]
    public class AnimationWave
    {
        public string triggerAnimationName;
        public Animation triggerAnimation;
        public List<Animation> animationsToPlay = new List<Animation>();
    }
    
    [Header("Configuration des vagues d'animations")]
    public List<AnimationWave> waves = new List<AnimationWave>();
    
    void Start()
    {
        foreach (AnimationWave wave in waves)
        {
            if (wave.triggerAnimation != null)
            {
                StartCoroutine(WatchAnimation(wave));
            }
        }
    }
    
    IEnumerator WatchAnimation(AnimationWave wave)
    {
        while (true)
        {
            yield return new WaitUntil(() => wave.triggerAnimation.IsPlaying(wave.triggerAnimationName));
            
            Debug.Log($"{wave.triggerAnimationName} commence...");
            
            yield return new WaitWhile(() => wave.triggerAnimation.IsPlaying(wave.triggerAnimationName));
            
            Debug.Log($"{wave.triggerAnimationName} terminée ! Lancement des animations suivantes...");
            
            foreach (Animation anim in wave.animationsToPlay)
            {
                if (anim != null)
                {
                    anim.Play();
                    Debug.Log($"Animation lancée sur: {anim.gameObject.name}");
                }
            }
        }
    }
}