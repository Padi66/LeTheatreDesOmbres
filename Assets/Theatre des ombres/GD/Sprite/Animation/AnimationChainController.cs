using UnityEngine;

public class AnimationChainController : StateMachineBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("Nom du paramètre trigger pour lancer la prochaine animation")]
    public string nextAnimationTrigger;
    
    [Tooltip("Délai avant de lancer la prochaine animation (en secondes)")]
    public float delay = 0f;
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Lancer la prochaine animation après le délai
    }
}
