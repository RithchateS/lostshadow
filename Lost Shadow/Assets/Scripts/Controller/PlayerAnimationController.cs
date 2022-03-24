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
    }
}
