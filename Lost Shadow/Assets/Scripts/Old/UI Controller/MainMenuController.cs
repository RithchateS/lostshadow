using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private MenuController controller;
    // Button Control
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject creditButton;
    [SerializeField] private GameObject exitButton;

    void Start()
    {
        playButton.GetComponent<Button>().onClick.AddListener(delegate
        {
            controller.ChangePage(1);
        });
        creditButton.GetComponent<Button>().onClick.AddListener(delegate
        {
            controller.ChangePage(2);
        });
        exitButton.GetComponent<Button>().onClick.AddListener(delegate
        {
            Application.Quit();
        });
    }

    
}
