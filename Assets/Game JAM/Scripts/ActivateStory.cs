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
    [SerializeField] private XRSocketInteractor _socketGreen;
    [SerializeField] private XRSocketInteractor _socketOrange;
    [SerializeField] private XRSocketInteractor _socketPurple;
    [SerializeField] private XRSocketInteractor _socketTool;

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
        LockAllCubesInSockets();
        StartCoroutine(Delay());
        StartCoroutine(MoveTicket());

    }
    
    private void LockAllCubesInSockets()
    {
        LockCubeInSocket(_socketGreen, "Socket Vert");
        LockCubeInSocket(_socketOrange, "Socket Orange");
        LockCubeInSocket(_socketPurple, "Socket Violet");
        LockCubeInSocket(_socketTool, "Socket Outil");
    }

    private void LockCubeInSocket(XRSocketInteractor socket, string socketName)
    {
        if (socket != null && socket.hasSelection)
        {
            IXRSelectInteractable interactable = socket.interactablesSelected[0];
            GameObject cube = interactable.transform.gameObject;
            
            XRGrabInteractable grabInteractable = cube.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null)
            {
                grabInteractable.enabled = false;
                Debug.Log($"Cube verrouillé dans {socketName}");
            }
        }
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
    
    IEnumerator Delay()
    {
        //texte d'attente de chargement
        yield return new WaitForSeconds(6f); 
    }
}
