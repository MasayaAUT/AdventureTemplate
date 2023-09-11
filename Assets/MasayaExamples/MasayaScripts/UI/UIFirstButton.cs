using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MasayaScripts.UI
{
    public class UIFirstButton : MonoBehaviour
    {
        public GameObject firstSelected;

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }
    }
}
