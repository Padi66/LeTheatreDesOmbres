using System.Collections;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class ActivateStory : MonoBehaviour
{
    [SerializeField] private XRPushButton _button;
    [SerializeField] private StoryManager _storyManager;
    

    private bool _hasBeenPressed = false;

    void OnEnable()
    {
        _button.onPress.AddListener(OnButtonPressed);
    }

    void OnDisable()
    {
        _button.onPress.RemoveListener(OnButtonPressed);
    }

    void OnButtonPressed()
    {
        if (!_storyManager._isLaunched)
        {
            _storyManager.CheckCombinationBackstage();
        }
        else
        {
            
        }
    }

  

}