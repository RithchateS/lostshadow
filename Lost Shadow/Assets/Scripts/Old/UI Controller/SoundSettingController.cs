using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public enum SettingState
{
    DefaultState = 1,
    UserState = 2,
    TempState = 3,
}
public class SoundSettingController : MonoBehaviour
{
    private SettingState _CurrectState;
    public SettingState CurrentState
    {
        get { return _CurrectState;}
        set
        {
            _CurrectState = value;
            OnStateChange();
        }
    }
    public Slider controlMasterAudio;
    public Slider controlMusicAudio;
    public Slider controlEffectAudio;
    public Slider controlAmbiantAudio;
    public AudioMixer audioMixer;
    public string masterAudioName;
    public string musicAudioName;
    public string effectAudioName;
    public string ambiantAudioName;
    public GameObject confirmButton;
    public GameObject cancelButton;
    public GameObject resetButton;
    public Setting InputData()
    {
        return new Setting
        {
            masterAudio = controlMasterAudio.value,
            effectAudio = controlEffectAudio.value,
            musicAudio  = controlMusicAudio.value,
            ambiantAudio = controlAmbiantAudio.value
        } ;
    }
    
    void OnEnable()
    {
        SetupSetting();
    }

    // void Start()
    // {        
    //     SetupSetting();
    // }

    public void ChangeState(SettingState state)
    {
        CurrentState = state;
    }

    public void OnStateChange()
    {
        confirmButton.SetActive(false);
        cancelButton.SetActive(false);
        resetButton.SetActive(false);

        switch (CurrentState)
        {
            case SettingState.DefaultState:
                controlMasterAudio.value = SettingManager.Instance.defaultValue.masterAudio;
                controlMusicAudio.value = SettingManager.Instance.defaultValue.musicAudio;
                controlEffectAudio.value = SettingManager.Instance.defaultValue.effectAudio;
                controlAmbiantAudio.value = SettingManager.Instance.defaultValue.ambiantAudio;
                MixerController();
                confirmButton.SetActive(true);
                cancelButton.SetActive(true);
                break;
            case SettingState.UserState:
                MixerController();
                if (SettingManager.Instance.userValue.masterAudio == SettingManager.Instance.defaultValue.masterAudio &&
                    SettingManager.Instance.userValue.musicAudio == SettingManager.Instance.defaultValue.musicAudio &&
                    SettingManager.Instance.userValue.effectAudio == SettingManager.Instance.defaultValue.effectAudio &&
                    SettingManager.Instance.userValue.ambiantAudio == SettingManager.Instance.defaultValue.ambiantAudio)
                {
                    resetButton.SetActive(false);
                }
                else
                {
                    resetButton.SetActive(true);
                }
                break;
            case SettingState.TempState:
                MixerController();
                confirmButton.SetActive(true);
                cancelButton.SetActive(true);
                resetButton.SetActive(true);   
                break;
        }

    }

    void SetupSetting()
    {
        controlMasterAudio.value = SettingManager.Instance.GetData.masterAudio;
        controlEffectAudio.value = SettingManager.Instance.GetData.effectAudio;
        controlMusicAudio.value = SettingManager.Instance.GetData.musicAudio;
        controlAmbiantAudio.value = SettingManager.Instance.GetData.ambiantAudio;
        MixerController();
    }
    
    public void ChangeValue()
    {
        MixerController();
        ChangeState(SettingState.TempState);
    }
    
    
    public void Confirm()
    {
        SettingManager.Instance.SetUserData(InputData());
        ChangeState(SettingState.UserState);
    }

    public void Cancel()
    {
        //
        controlMasterAudio.value = SettingManager.Instance.GetData.masterAudio;
        controlMusicAudio.value = SettingManager.Instance.GetData.musicAudio;
        controlEffectAudio.value = SettingManager.Instance.GetData.effectAudio;
        controlAmbiantAudio.value = SettingManager.Instance.GetData.ambiantAudio;
        ChangeState(SettingState.UserState);
    }

    public void ToDefault()
    {
        ChangeState(SettingState.DefaultState);
    }

    public void MixerController()
    {
        audioMixer.SetFloat(masterAudioName,controlMasterAudio.value);
        audioMixer.SetFloat(musicAudioName,controlMusicAudio.value);
        audioMixer.SetFloat(effectAudioName,controlEffectAudio.value);
        audioMixer.SetFloat(ambiantAudioName,controlAmbiantAudio.value);
        //
        if (controlMasterAudio.value == -20)
        {
            audioMixer.SetFloat(masterAudioName,-80);
        }
        if (controlEffectAudio.value == -20)
        {
            audioMixer.SetFloat(effectAudioName,-80);
        }
        if (controlMusicAudio.value == -20)
        {
            audioMixer.SetFloat(musicAudioName,-80);
        }
        if (controlAmbiantAudio.value == -20)
        {
            audioMixer.SetFloat(ambiantAudioName,-80);
        }

    }
}