using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace MasayaScripts
{
    public class LoadScene : MonoBehaviour
    {
        bool playerFound;
        public string sceneName;
        public bool setLevelLoadPosition;
        public Vector3 levelLoadPosition;

        public UnityEvent enterEvent;
        public UnityEvent exitEvent;

        private void Update()
        {
            if (playerFound && Input.GetKeyDown(KeyCode.E))
            {
                if (setLevelLoadPosition)
                {
                    StartLevelLocation.setLocation = true;
                    StartLevelLocation.position = levelLoadPosition;
                }

                SceneManager.LoadScene(sceneName);

            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                playerFound = true;
                enterEvent.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                playerFound = false;
                exitEvent.Invoke();
            }
        }
    }
}
