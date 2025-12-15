using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

public class LaunchStoryScene : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private DialogueSequence _dialogueSequence;
    [SerializeField] private AudioSource _audioSource;
    
    [SerializeField] private AnimationDelay _animationDelay;
    [SerializeField] private GameObject _projecteur;
    
    [Header("Lights")]
    [SerializeField] private Light[] _lights;
    [SerializeField] private float _lightFadeDuration = 2f;
    
    [Header("Delay")]
    [SerializeField] private float _extraDelayAfterCompletion = 1f;
    [SerializeField] private float delayBeforeTransition = 1f;

    [Header("Scene Transition")]
    
    [SerializeField] private int _mainMenuSceneIndex = 0;
    
    void Start()
    {
        StartCoroutine(EnsureMovementStaysDisabled());
        StartCoroutine(Sequence());
    }

    private IEnumerator EnsureMovementStaysDisabled()
    {
        yield return new WaitForSeconds(0.5f);
        
        while (true)
        {
            DisableAllMovement();
            DisableControllerRays();
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void DisableAllMovement()
    {
        ContinuousMoveProvider[] moveProviders = FindObjectsByType<ContinuousMoveProvider>(FindObjectsSortMode.None);
        ContinuousTurnProvider[] continuousTurnProviders = FindObjectsByType<ContinuousTurnProvider>(FindObjectsSortMode.None);
        SnapTurnProvider[] snapTurnProviders = FindObjectsByType<SnapTurnProvider>(FindObjectsSortMode.None);
        
        foreach (var move in moveProviders)
        {
            if (move.enabled)
            {
                move.enabled = false;
            }
        }

        foreach (var turn in continuousTurnProviders)
        {
            if (turn.enabled)
            {
                turn.enabled = false;
            }
        }

        foreach (var snapTurn in snapTurnProviders)
        {
            if (snapTurn.enabled)
            {
                snapTurn.enabled = false;
            }
        }
    }

    private void DisableControllerRays()
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "LineVisual" && obj.activeSelf)
            {
                obj.SetActive(false);
            }
        }
    }

    IEnumerator Sequence()
    {
        yield return new WaitForSecondsRealtime(5f);
        
        _animationDelay.ResumeAnimations();
        _projecteur.SetActive(true);
        StartCoroutine(FadeLightsOut(_lightFadeDuration));
        _dialogueSequence.StartDialogueBranch(12);
        _audioSource.Play();
        
        yield return StartCoroutine(WaitForDialoguesAndAudio());

        yield return new WaitForSeconds(_extraDelayAfterCompletion);
        _projecteur.SetActive(false);
        StartCoroutine(FadeLightsIn(_lightFadeDuration));

        yield return new WaitForSeconds(_lightFadeDuration);

        StartCoroutine(TransitionAfterDelay(_mainMenuSceneIndex));
    }

    private IEnumerator WaitForDialoguesAndAudio()
    {
        while (_audioSource.isPlaying)
        {
            yield return null;
        }
    }

    IEnumerator FadeLightsOut(float duration)
    {
        float[] startIntensities = new float[_lights.Length];
        
        for (int i = 0; i < _lights.Length; i++)
        {
            if (_lights[i] != null)
            {
                startIntensities[i] = _lights[i].intensity;
            }
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            
            for (int i = 0; i < _lights.Length; i++)
            {
                if (_lights[i] != null)
                {
                    _lights[i].intensity = Mathf.Lerp(startIntensities[i], 0f, t);
                }
            }
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        foreach (var light in _lights)
        {
            if (light != null)
            {
                light.intensity = 0f;
            }
        }
    }

    IEnumerator FadeLightsIn(float duration)
    {
        float[] targetIntensities = new float[_lights.Length];
        
        for (int i = 0; i < _lights.Length; i++)
        {
            if (_lights[i] != null)
            {
                targetIntensities[i] = 1f;
            }
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            
            for (int i = 0; i < _lights.Length; i++)
            {
                if (_lights[i] != null)
                {
                    _lights[i].intensity = Mathf.Lerp(0f, targetIntensities[i], t);
                }
            }
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        foreach (var light in _lights)
        {
            if (light != null)
            {
                light.intensity = 1f;
            }
        }
    }
    
    private IEnumerator TransitionAfterDelay(int sceneIndex)
    {
        Debug.Log($"[LaunchStoryScene] Waiting {delayBeforeTransition}s before transition...");
        yield return new WaitForSeconds(delayBeforeTransition);

        Debug.Log($"[LaunchStoryScene] Starting transition to scene {sceneIndex}");
    
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.StartCoroutine(
                SceneTransitionManager.Instance.TransitionToScene(sceneIndex, disableMovement: false)
            );
        }
        else
        {
            Debug.LogError("[LaunchStoryScene] SceneTransitionManager.Instance is null!");
        }
    }


}
