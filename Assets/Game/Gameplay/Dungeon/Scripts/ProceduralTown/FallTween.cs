using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace SVS
{
    public class FallTween : MonoBehaviour
    {
        private Vector3 destination;
        public float timeToFall = 0.2f;

        private void Start() 
        {
            destination = transform.position;
            gameObject.transform.position  += Vector3.up * 10;
            
            StartCoroutine(DropDown());
        }

        private IEnumerator DropDown()
        {
            Vector3 position =  gameObject.transform.position;
            float currentTime = 0f;

            do
            {
              gameObject.transform.position = Vector3.Lerp(position, destination, currentTime / timeToFall);
              currentTime += Time.deltaTime;

              yield return null;
            }while (currentTime <= timeToFall);
            gameObject.transform.position = destination;


        }

    }
}