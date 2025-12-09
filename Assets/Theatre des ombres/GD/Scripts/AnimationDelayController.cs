using UnityEngine;

public class AnimationDelayController : MonoBehaviour
{
    [Header("Pause Settings")]
    [SerializeField] private float delayBeforeStart = 2f;
    
    private void Start()
    {
        Time.timeScale = 0f;
        Invoke(nameof(ResumeTime), delayBeforeStart);
    }
    
    private void ResumeTime()
    {
        Time.timeScale = 1f;
    }
}