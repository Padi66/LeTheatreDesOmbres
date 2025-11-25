using System.Collections;
using UnityEngine;

public class LaunchStory : MonoBehaviour
{
    [SerializeField] DialogueSequence _dialogueSequence;
    [SerializeField] CurtainsMove _curtains;
    [SerializeField] Light _light;
    
    
    void Start()
    {
        _light.intensity = 1;
        StartCoroutine(Delay());
        _curtains.OpenCurtainsRight();
        _curtains.OpenCurtainsLeft();
        StartCoroutine(Delay());
        _dialogueSequence.StartDialogueBranch(23);
        
    }


    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);

    }
}
 