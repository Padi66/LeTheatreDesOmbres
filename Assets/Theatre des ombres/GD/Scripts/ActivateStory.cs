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
        
        if (_buttonVisual == null)
        {
            _buttonVisual = gameObject;
        }
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
            SetLayerRecursively(_buttonVisual, _outlineLayer);
            Debug.Log("Tous les sockets occupés - Outline activé!");
        }
        else
        {
            SetLayerRecursively(_buttonVisual, _defaultLayer);
            Debug.Log("Outline désactivé");
        }
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        if (obj == null) return;
        
        obj.layer = layer;
        
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    void OnButtonPressed()
    {
        if (_hasBeenPressed)
        {
            Debug.LogWarning("Le bouton a déjà été appuyé!");
            return;
        }

        if (!_allSocketsOccupied)
        {
            Debug.LogWarning("Tous les sockets doivent être occupés!");
            Debug.Log($"Green: {_storyManager._socketGreen}, Orange: {_storyManager._socketOrange}, Purple: {_storyManager._socketPurple}");
            return;
        }

        if (!_storyManager._isLaunched)
        {
            _hasBeenPressed = true;
            SetLayerRecursively(_buttonVisual, _defaultLayer);
            _storyManager.CheckCombinationBackstage();
            Debug.Log("Bouton appuyé!");
        }
    }

    public void ResetButton()
    {
        _hasBeenPressed = false;
        UpdateButtonOutline();
        Debug.Log("Bouton réinitialisé");
    }
}
