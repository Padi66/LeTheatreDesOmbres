using UnityEngine;

public class TriggerOnAnimationEnd : StateMachineBehaviour
{
    [Header("Trigger Settings")]
    [Tooltip("Nom du trigger à activer quand l'animation se termine")]
    public string triggerName = "TransitionSlice";
    
    [Tooltip("Délai optionnel avant d'activer le trigger (en secondes)")]
    public float delay = 0f;
    
    private float exitTime;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        exitTime = -1f;
    }
    
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (delay > 0f && exitTime < 0f && stateInfo.normalizedTime >= 1f)
        {
            exitTime = Time.time;
        }
        
        if (exitTime > 0f && Time.time >= exitTime + delay)
        {
            animator.SetTrigger(triggerName);
            exitTime = -1f;
        }
    }
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (delay == 0f)
        {
            animator.SetTrigger(triggerName);
        }
    }
}