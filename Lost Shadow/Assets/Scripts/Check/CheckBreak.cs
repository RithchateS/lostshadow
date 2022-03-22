using UnityEngine;

public class CheckBreak : MonoBehaviour
{
    public void EndBreak(string s)
    {
        if (s == "EndBreak")
        {
            Destroy(gameObject);
        }
    }
}
