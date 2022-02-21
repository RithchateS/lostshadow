using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private MenuController controller;
    [SerializeField] private bool lightTheme = true;
    // Button Control
    [SerializeField] private GameObject newGame_button;
    [SerializeField] private GameObject setting_button;
    [SerializeField] private GameObject theme_button;
    [SerializeField] private GameObject exit_button;
    // Element Collectter
    [SerializeField] private Image[] button_Image;
    [SerializeField] private SpriteRenderer[] button_Renderer;
    [SerializeField] private SpriteRenderer bg_Renderer;
    [SerializeField] private Sprite darkButton_Sprite;
    [SerializeField] private Sprite darkBg_Sprite;
    [SerializeField] private Sprite lightButton_Sprite;
    [SerializeField] private Sprite lightBg_Sprite;
    
    void Start()
    {
        newGame_button.GetComponent<Button>().onClick.AddListener(delegate
        {
            controller.ChangePage(1);
        });
        setting_button.GetComponent<Button>().onClick.AddListener(delegate
        {
            controller.ChangePage(3);
        });
        theme_button.GetComponent<Button>().onClick.AddListener(delegate
        {
            ChangeTheme();
        });
        exit_button.GetComponent<Button>().onClick.AddListener(delegate
        {
            Application.Quit();
        });
    }

    void ChangeTheme()
    {
        lightTheme = !lightTheme;
        if (lightTheme)
        {
            foreach (var button in button_Image)
            {
                button.sprite = lightButton_Sprite;
            }

            bg_Renderer.sprite = lightBg_Sprite;
        }
        else if (!lightTheme)
        {
            foreach (var button in button_Image)
            {
                button.sprite = darkButton_Sprite;
            }
            bg_Renderer.sprite = darkBg_Sprite;
        }
        Debug.Log("Done");
    }
}
