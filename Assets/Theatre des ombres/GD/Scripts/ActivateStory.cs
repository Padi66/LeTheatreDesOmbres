using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class ActivateStory : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private XRPushButton _button;
    [SerializeField] private StoryManager _storyManager;
    [SerializeField] private GameObject _buttonVisual;

    private bool _hasBeenPressed = false;
    private bool _allSocketsOccupied = false;
    
    private int _outlineLayer;
    private int _defaultLayer;

    void Awake()
    {
        _outlineLayer = LayerMask.NameToLayer("Outline");
        _defaultLayer = LayerMask.NameToLayer("Default");
        
        
        
        
    }

    void OnEnable()
    {
        _button.onPress.AddListener(OnButtonPressed);
    }

    void OnDisable()
    {
        _button.onPress.RemoveListener(OnButtonPressed);
    }

    void Update()
    {
        CheckSocketsState();
    }

    private void CheckSocketsState()
    {
        bool previousState = _allSocketsOccupied;
        _allSocketsOccupied = _storyManager._socketGreen && 
                              _storyManager._socketOrange && 
                              _storyManager._socketPurple;

        if (previousState != _allSocketsOccupied)
        {
            UpdateButtonOutline();
        }
    }

    private void UpdateButtonOutline()
    {
        if (_allSocketsOccupied && !_hasBeenPressed)
        {
            _buttonVisual.layer = _outlineLayer;
            
        }
        else
        {
            _buttonVisual.layer = _defaultLayer;
            
        }
    }

    void OnButtonPressed()
    {
        
        if (!_storyManager._isLaunched)
        {
            _hasBeenPressed = true;
            _buttonVisual.layer = _defaultLayer;
            _storyManager.CheckCombinationBackstage();
            
        }
    }

    public void ResetButton()
    {
        _hasBeenPressed = false;
        UpdateButtonOutline();
        ;
    }
}
