using System.Collections;
using UnityEngine;

public class CurtainsMove : MonoBehaviour
{
    public Transform _startPositionLeft;
    public Transform _endPositionLeft;
    public Transform _startPositionRight;
    public Transform _endPositionRight;
    public float _duration = 2f;
    
    public void OpenCurtainsLeft()
    {
        //SON
        StartCoroutine(OpenLeft());
    }
    public void CloseCurtainsLeft()
    {
        //SON
        StartCoroutine(CloseLeft());
    }
    public void OpenCurtainsRight()
    {
        //SON
        StartCoroutine(OpenRight());
    }
    public void CloseCurtainsRight()
    {
        //SON
        StartCoroutine(CloseRight());
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
    IEnumerator OpenRight()
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            transform.position = Vector3.Lerp(_startPositionRight.position, _endPositionRight.position, elapsed / _duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = _endPositionRight.position;
    }
    IEnumerator CloseRight()
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            transform.position = Vector3.Lerp(_endPositionRight.position, _startPositionRight.position, elapsed / _duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = _startPositionRight.position;
    }
   
}


