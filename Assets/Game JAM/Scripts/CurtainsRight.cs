using System.Collections;
using UnityEngine;

public class CurtainsRight : MonoBehaviour
{
   public Transform _startPositionRight;
   public Transform _endPositionRight;
   public float _duration = 2f;

   public void OpenCurtains()
   {
      //SON
      StartCoroutine(OpenRight());
   }
   public void CloseCurtains()
   {
      //SON
      StartCoroutine(CloseRight());
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

