using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MasayaScripts
{
    public class AnimationToggle : MonoBehaviour
    {

        Animator anim;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextAnimation();
            }
        }

        public void NextAnimation()
        {
            anim.SetTrigger("NextAnimation");
        }
    }
}
