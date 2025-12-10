using System.Collections;
using UnityEngine;

public class AnimationDelayController : MonoBehaviour
{
    [Header("Pause Settings")]
    [SerializeField] public float delayBeforeStart = 2f;
    
    private void Start()
    {
        Time.timeScale = 0f;
        StartCoroutine(ResumeTimeAfterDelay());
    }
    
    private IEnumerator ResumeTimeAfterDelay()
    {
        yield return new WaitForSecondsRealtime(delayBeforeStart);
        Time.timeScale = 1f;
    }
}