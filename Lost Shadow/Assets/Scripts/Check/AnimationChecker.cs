using Manager;
using UnityEngine;

namespace Controller
{
    public class AnimationChecker : MonoBehaviour
    {
        public void PlayerEndWakeUp(string s)
        {
            if (s == "EndWakeUp")
            {
                LevelManager.Instance.cameraObj.GetComponent<Animator>().SetBool("Cutscene", false);
            }
        }
        
        public void PlayerEndShift(string s)
        {
            if (s == "EndShift")
            {
                gameObject.GetComponent<Animator>().SetBool("isShifting", false);
            }
        }

        public void EndBreak(string s)
        {
            if (s == "EndBreak")
            {
                Destroy(gameObject);
            }
        }


    }
}
