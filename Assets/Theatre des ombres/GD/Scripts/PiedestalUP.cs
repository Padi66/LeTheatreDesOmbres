using System;
using System.Collections;
using UnityEngine;

public class PiedestalUP : MonoBehaviour
{
 
    public Transform _startPositionGreen;
    public Transform _endPositionGreen;
    public Transform _startPositionPurple;
    public Transform _endPositionPurple;
    public Transform _startPositionOrange;
    public Transform _endPositionOrange;
    public Transform _piedestalOrange;
    public Transform _piedestalPurple;
    public Transform _piedestalGreen;
    
    public float _duration = 2f;

   
    public void UpGreen()
    {
        //SON
        StartCoroutine(UpEnumGreen());
    }
    public void DownGreen()
    {
        //SON
        StartCoroutine(DownEnumGreen());
    }
    public void UpPurple()
    {
        //SON
        StartCoroutine(UpEnumPurple());
    }
    public void DownPurple()
    {
        //SON
        StartCoroutine(DownEnumPurple());
    }
    
    public void UpOrange()
    {
        Debug.Log("PPI");
        //SON
        StartCoroutine(UpEnumOrange());
    }
    public void DownOrange()
    {
        //SON
        StartCoroutine(DownEnumOrange());
    }
    
    IEnumerator UpEnumGreen()
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            _piedestalGreen.position = Vector3.Lerp(_startPositionGreen.position, _endPositionGreen.position, elapsed / _duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _piedestalGreen.position = _endPositionGreen.position;
    }
    IEnumerator DownEnumGreen()
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            _piedestalGreen.position = Vector3.Lerp(_endPositionGreen.position, _startPositionGreen.position, elapsed / _duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _piedestalGreen.position = _startPositionGreen.position;
    }
    IEnumerator UpEnumPurple()
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            _piedestalPurple.position = Vector3.Lerp(_startPositionPurple.position, _endPositionPurple.position, elapsed / _duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

      _piedestalPurple.position = _endPositionPurple.position;
    }
    IEnumerator DownEnumPurple()
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            _piedestalPurple.position = Vector3.Lerp(_endPositionPurple.position, _startPositionPurple.position, elapsed / _duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _piedestalPurple.position = _startPositionPurple.position;
    }
    
    IEnumerator UpEnumOrange()
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            _piedestalOrange.position = Vector3.Lerp(_startPositionOrange.position, _endPositionOrange.position, elapsed / _duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _piedestalOrange.position = _endPositionOrange.position;
    }
    IEnumerator DownEnumOrange()
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            _piedestalOrange.position = Vector3.Lerp(_endPositionOrange.position, _startPositionOrange.position, elapsed / _duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _piedestalOrange.position = _startPositionOrange.position;
    }
}

