using System.Collections;
using UnityEngine;

public class LaunchStory : MonoBehaviour
{
    [SerializeField] DialogueSequence _dialogueSequence;
    [SerializeField] CurtainsMove _curtains;
    [SerializeField] LevelManager _levelManager;
    //[SerializeField] Light _light;
    
    
    void Start()
    {
        //_light.intensity = 1;
        StartCoroutine(Delay(6));
        _curtains.OpenCurtainsRight();
        _curtains.OpenCurtainsLeft();
        StartCoroutine(Delay(3));
        _dialogueSequence.StartDialogueBranch(12);
        StartCoroutine(Delay(3));
        _levelManager.LoadBackStage();
        
        
    }


    IEnumerator Delay(int delay)
    {
        yield return new WaitForSeconds(delay);

    }
}
 