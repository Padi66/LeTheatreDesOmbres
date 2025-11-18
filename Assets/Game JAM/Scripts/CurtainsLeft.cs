using System.Collections;
using UnityEngine;

public class CurtainsLeft : MonoBehaviour
{
    public Transform _startPositionLeft;
    public Transform _endPositionLeft;
    public float _duration = 2f;
    
    public void OpenCurtains()
    {
        //SON
        StartCoroutine(OpenLeft());
    }
    public void CloseCurtains()
    {
        //SON
        StartCoroutine(CloseLeft());
    }
    
    
    IEnumerator OpenLeft()
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            transform.position = Vector3.Lerp(_startPositionLeft.position, _endPositionLeft.position, elapsed / _duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = _endPositionLeft.position;
    }
    IEnumerator CloseLeft()
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            transform.position = Vector3.Lerp(_endPositionLeft.position, _startPositionLeft.position, elapsed / _duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = _startPositionLeft.position;
    }
}

