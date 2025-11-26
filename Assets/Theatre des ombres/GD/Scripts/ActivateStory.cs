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
    
    [SerializeField] private SocketPurple _socketPurpleRef;

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
        if (_socketPurpleRef._isInSocket)
        {
            
            LockAllCubesInSockets();
            StartCoroutine(Delay());
            StartCoroutine(MoveTicket());
            StoryManager.OnPushButton?.Invoke();
        }
    }
    
    private void LockAllCubesInSockets()
    {
        XRSocketInteractor socketInteractor = _socketPurpleRef.GetComponent<XRSocketInteractor>();
        LockCubeInSocket(socketInteractor, "Socket Violet");
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

            Rigidbody rb = cube.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                Debug.Log($"Rigidbody désactivé dans {socketName}");
            }
        }
    }

    IEnumerator MoveTicket()
    {
        Debug.Log("Marche Pas ou pas loumpa");
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            _socketAttach.position = Vector3.Lerp(
                _attachPositionStart.position, 
                _attachPositionEnd.position,
                elapsed / _duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        _socketAttach.position = _attachPositionEnd.position;
    }
    
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(5f); 
    }
}
