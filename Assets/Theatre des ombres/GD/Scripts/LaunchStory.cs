using System.Collections;
using UnityEngine;

public class LaunchStory : MonoBehaviour
{
    [SerializeField] DialogueSequence _dialogueSequence;
    [SerializeField] CurtainsMove _curtains;
    [SerializeField] LevelManager _levelManager;
    [SerializeField] private LightGroup _lightGroup;
    
    
    void Start()
    {
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        yield return new WaitForSeconds(6f);

        _curtains.OpenCurtainsRight();
        _curtains.OpenCurtainsLeft();

        yield return new WaitForSeconds(6f);

        _dialogueSequence.StartDialogueBranch(12);

        yield return new WaitForSeconds(6f);

        //_light.intensity = 1
        _levelManager.LoadBackStage();
    }
}
 