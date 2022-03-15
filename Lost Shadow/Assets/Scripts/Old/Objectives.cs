using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Objectives : MonoBehaviour
{
    public GameObject objectives;

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            objectives.GetComponent<TextMeshProUGUI>().text = "Follow Tutorial";
        }
        else if (SceneManager.GetActiveScene().buildIndex == 10)
        {
            objectives.GetComponent<TextMeshProUGUI>().text = "Explore The Village and then Reach the Farthest Right Corner";
        }
        else if (SceneManager.GetActiveScene().buildIndex is 7 or 6)
        {
            objectives.GetComponent<TextMeshProUGUI>().text = "Explore The Shadow Forest/ Reach The Farthest Right Corner";
        }
        else if (SceneManager.GetActiveScene().buildIndex is 8 or 9)
        {
            objectives.GetComponent<TextMeshProUGUI>().text = "Get Items and Avoid Guards (Items Represents Green Bush)";
        }
    }
}
