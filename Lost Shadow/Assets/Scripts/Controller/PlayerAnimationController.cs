using Manager;
using UnityEngine;

namespace Controller
{
    public class PlayerAnimationController : MonoBehaviour
    {
        public void EndWakeUp(string s)
        {
            if (s == "EndWakeUp")
            {
                LevelManager.Instance.cameraObj.GetComponent<Animator>().SetBool("Cutscene", false);
            }
        }
        
        public void EndPeek(string s)
        {
            if (s == "EndPeek")
            {
                gameObject.GetComponent<Animator>().SetBool("isPeeking",false);
            }
        }
        public void EndShift(string s)
        {
            if (s == "EndShift")
            {
                gameObject.GetComponent<Animator>().SetBool("isShifting" ,false);
            }
        }
        
    }
}
