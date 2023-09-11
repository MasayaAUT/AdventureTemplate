using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MasayaScripts
{
    public class LoadScene : MonoBehaviour
    {
        bool playerFound;
        public string sceneName;
        public bool setLevelLoadPosition;
        public Vector3 levelLoadPosition;

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
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                playerFound = false;
            }
        }
    }
}
