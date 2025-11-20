using System.Collections;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ActivateStory : MonoBehaviour
{
    [SerializeField] private XRPushButton _button;
    [SerializeField] private Transform _attachPositionStart;
    [SerializeField] private Transform _attachPositionEnd;
    [SerializeField] private Transform _socketAttach; 
    [SerializeField] private float _duration;
    [SerializeField] private XRGrabInteractable _grabInteractable;

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
        StoryManager.OnPushButton?.Invoke();
        _grabInteractable.enabled = false;
        StartCoroutine(MoveTicket());

    }

    IEnumerator MoveTicket()
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            _socketAttach.position = Vector3.Lerp(
                _attachPositionStart.position, 
                _attachPositionEnd.position,
                elapsed / _duration
            );
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
