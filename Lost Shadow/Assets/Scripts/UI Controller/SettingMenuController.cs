using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public enum SettingPage
{
    General,
    Sound,
    Control
}

public class SettingMenuController : MonoBehaviour
{
    // Main Button
    [SerializeField] private GameObject general_Button;
    [SerializeField] private GameObject sound_Button;
    [SerializeField] private GameObject control_Button;
    [SerializeField] private GameObject fullToggle;
    // Main Object
    [SerializeField] private GameObject generalPage;
    [SerializeField] private GameObject soundPage;
    [SerializeField] private GameObject controlPage;
    // General
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] _resolutions;
    private SettingPage _CurrentPage;
    public SettingPage CurrentPage
    {
        get { return _CurrentPage; }
        set
        {
            _CurrentPage = value;
            OnPageChange();
        }
    }

    public void ChangePage(SettingPage page)
    {
        CurrentPage = page;
    }

    public void ChangePage(int pageIndex)
    {
        CurrentPage = (SettingPage) pageIndex;
    }

    private void OnPageChange()
    {
        generalPage.SetActive(false);
        soundPage.SetActive(false);
        controlPage.SetActive(false);

        switch (CurrentPage)
        {
            case SettingPage.General:
                generalPage.SetActive(true);
                soundPage.SetActive(false);
                controlPage.SetActive(false);
                break;
            case SettingPage.Sound:
                generalPage.SetActive(false);
                soundPage.SetActive(true);
                controlPage.SetActive(false);
                break;
            case SettingPage.Control:
                generalPage.SetActive(false);
                soundPage.SetActive(false);
                controlPage.SetActive(true);
                break;
        }
    }

    private void Start()
    {
        _resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height;
            options.Add(option);

            if (_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        
        fullToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate
        {
            setFullScreen(fullToggle.GetComponent<Toggle>().isOn);
        });
        general_Button.GetComponent<Button>().onClick.AddListener(delegate
        {
            ChangePage(SettingPage.General);
        });
        sound_Button.GetComponent<Button>().onClick.AddListener(delegate
        {
            ChangePage(SettingPage.Sound);
        });
        control_Button.GetComponent<Button>().onClick.AddListener(delegate
        {
            ChangePage(SettingPage.Control);
        });
        
    }

    void Confirm()
    {
        
    }

    void ChangeResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width ,resolution.height, Screen.fullScreen);
    }

    void setFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SettingMenuController))]
public class SettingControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SettingMenuController Page = (SettingMenuController) target;

        if (GUILayout.Button("General"))
        {
            Page.ChangePage(SettingPage.General);
        }

        if (GUILayout.Button("Sound"))
        {
            Page.ChangePage(SettingPage.Sound);
        }

        if (GUILayout.Button("Control"))
        {
            Page.ChangePage(SettingPage.Control);
        }
        
    }
}
#endif
