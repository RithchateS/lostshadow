
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class TransitionController : MonoBehaviour
    {
        
        private Animator _animator;
        public static TransitionController Instance;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            _animator = transform.GetChild(0).GetComponent<Animator>();
        }

        public IEnumerator EndTransition()
        {
            yield return new WaitForSeconds(2.5f);
            _animator.SetTrigger("End");
        }
        public void StartTransition()
        {
            _animator.SetTrigger("Start");
        }
    }
}
