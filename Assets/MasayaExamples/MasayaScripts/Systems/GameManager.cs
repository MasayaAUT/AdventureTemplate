using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MasayaScripts
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager current;

        // Start is called before the first frame update
        void Awake()
        {
            if (current == null)
            {
                current = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
