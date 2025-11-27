using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

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

   
    public void UpOrange(XRLockSocketInteractor socketToReactivate = null)
    {
        StartCoroutine(UpEnumOrange(socketToReactivate));
    }

    public void UpGreen(XRLockSocketInteractor socketToReactivate = null)
    {
        StartCoroutine(UpEnumGreen(socketToReactivate));
    }

    public void UpPurple(XRLockSocketInteractor socketToReactivate = null)
    {
        StartCoroutine(UpEnumPurple(socketToReactivate));
    }
    
    IEnumerator UpEnumGreen(XRLockSocketInteractor socketToReactivate = null)
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            _piedestalGreen.position = Vector3.Lerp(_startPositionGreen.position, _endPositionGreen.position, elapsed / _duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _piedestalGreen.position = _endPositionGreen.position;

        if (socketToReactivate != null)
        {
            socketToReactivate.enabled = true;
            Debug.Log("Socket Green réactivé après la montée du piédestal");
        }
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
    IEnumerator UpEnumPurple(XRLockSocketInteractor socketToReactivate = null)
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            _piedestalPurple.position = Vector3.Lerp(_startPositionPurple.position, _endPositionPurple.position, elapsed / _duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _piedestalPurple.position = _endPositionPurple.position;

        if (socketToReactivate != null)
        {
            socketToReactivate.enabled = true;
            Debug.Log("Socket Purple réactivé après la montée du piédestal");
        }
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
    
    IEnumerator UpEnumOrange(XRLockSocketInteractor socketToReactivate = null)
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            _piedestalOrange.position = Vector3.Lerp(_startPositionOrange.position, _endPositionOrange.position, elapsed / _duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    
        _piedestalOrange.position = _endPositionOrange.position;

        if (socketToReactivate != null)
        {
            socketToReactivate.enabled = true;
            Debug.Log("Socket Orange réactivé après la montée du piédestal");
        }
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

