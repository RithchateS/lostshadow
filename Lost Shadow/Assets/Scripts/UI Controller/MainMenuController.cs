using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private MenuController controller;
    // Button Control
    [SerializeField] private GameObject newGame_button;
    [SerializeField] private GameObject loadGame_button;
    [SerializeField] private GameObject setting_button;
    [SerializeField] private GameObject wtf_button;
    [SerializeField] private GameObject exit_button;
    void Start()
    {
        newGame_button.GetComponent<Button>().onClick.AddListener(delegate
        {
            controller.ChangePage(1);
        });
        loadGame_button.GetComponent<Button>().onClick.AddListener(delegate
        {
            controller.ChangePage(2);
        });
        setting_button.GetComponent<Button>().onClick.AddListener(delegate
        {
            controller.ChangePage(3);
        });
        wtf_button.GetComponent<Button>().onClick.AddListener(delegate
        {
            
        });
        exit_button.GetComponent<Button>().onClick.AddListener(delegate
        {
            Application.Quit();
        });
    }
    
    
    void Update()
    {
        
    }
}
