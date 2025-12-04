using System.Collections;
using UnityEngine;

public class LaunchStoryScene : MonoBehaviour
{
    [SerializeField] DialogueSequence _dialogueSequence;
    [SerializeField] CurtainsMove _curtains;
    [SerializeField] LevelManager _levelManager;
    [SerializeField] private LightGroup _lightGroup;
    [SerializeField] AudioSource _audioSource;
    
    
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
        _audioSource.Play();

        yield return new WaitForSeconds(6f);

        //_light.intensity = 1
        //_levelManager.LoadBackStage();
    }
}
 