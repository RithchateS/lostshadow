using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Menulist
{
    Main,
    New,
    Load,
    Setting
}

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject back_Button;
    private Menulist _CurrentPage;
    public Menulist CurrentPage
    {
        get { return _CurrentPage; }
        set
        {
            _CurrentPage = value;
            OnMenuListChange();
        }
    }
    [SerializeField] private TMP_Text pageTitle;
    // GameObject
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject newGamePage;
    [SerializeField] private GameObject loadGamePage;
    [SerializeField] private GameObject settingPage;

    private void Start()
    {
        back_Button.GetComponent<Button>().onClick.AddListener(delegate
        {
            ChangePage(0);
        });
        pageTitle.text = "";
    }

    public void ChangePage(Menulist page)
    {
        CurrentPage = page;
    }
    
    public void ChangePage(int pageIndex)
    { 
        CurrentPage = (Menulist)pageIndex;
    }
    
    private void OnMenuListChange()
    {
        mainMenu.SetActive(false);
        settingPage.SetActive(false);
        loadGamePage.SetActive(false);
        newGamePage.SetActive(false);
        back_Button.SetActive(false);
        
        switch (CurrentPage)
        {
            case Menulist.Main:
                mainMenu.SetActive(true);
                back_Button.SetActive(false);
                pageTitle.text = "";
                break;
            case Menulist.New:
                newGamePage.SetActive(true);
                back_Button.SetActive(true);
                pageTitle.text = "New Game";
                break;
            case Menulist.Load:
                loadGamePage.SetActive(true);
                back_Button.SetActive(true);
                pageTitle.text = "Load Game";
                break;
            case Menulist.Setting:
                settingPage.SetActive(true);
                back_Button.SetActive(true);
                pageTitle.text = "Setting";
                break;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MenuController))]
    public class MainMenuControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            MenuController menu = (MenuController) target;

            if (GUILayout.Button("Main"))
            {
                menu.ChangePage(Menulist.Main);
            }

            if (GUILayout.Button("New"))
            {
                menu.ChangePage(Menulist.New);
            }

            if (GUILayout.Button("Load"))
            {
                menu.ChangePage(Menulist.Load);
            }

            if (GUILayout.Button("Setting"))
            {
                menu.ChangePage(Menulist.Setting);
            }

        }
    }
}
#endif