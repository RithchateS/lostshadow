using System.Collections;
using Controller;
using Manager;
using Old.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayMenuController : MonoBehaviour
{
    [SerializeField]private GameObject saveSlot1;
    [SerializeField]private GameObject saveSlot2;
    [SerializeField]private GameObject saveSlot3;
    [SerializeField] private GameObject deleteSave1;
    [SerializeField] private GameObject deleteSave2;
    [SerializeField] private GameObject deleteSave3;
    private void Start()
    {
        Appdata.Instance.currentScene = SceneCollection.MainMenu;
        deleteSave1.GetComponent<Button>().onClick.AddListener(delegate
        {
            GameSaveManager.Instance.DeleteSave(1);
        });deleteSave2.GetComponent<Button>().onClick.AddListener(delegate
        {
            GameSaveManager.Instance.DeleteSave(2);
        });deleteSave3.GetComponent<Button>().onClick.AddListener(delegate
        {
            GameSaveManager.Instance.DeleteSave(3);
        });
        saveSlot1.GetComponent<Button>().onClick.AddListener(delegate
        {
            GameSaveManager.Instance.SelectSaveSlot(0);
            Appdata.Instance.sceneToLoad = Appdata.Instance.SceneInSave1;
            Appdata.Instance.currentScene = Appdata.Instance.sceneToLoad;
            StartCoroutine(PlayGame());
        });
        saveSlot2.GetComponent<Button>().onClick.AddListener(delegate
        {
            GameSaveManager.Instance.SelectSaveSlot(1);
            Appdata.Instance.sceneToLoad = Appdata.Instance.SceneInSave2;
            Appdata.Instance.currentScene = Appdata.Instance.sceneToLoad;
            StartCoroutine(PlayGame());
        });
        saveSlot3.GetComponent<Button>().onClick.AddListener(delegate
        {
            GameSaveManager.Instance.SelectSaveSlot(2);
            Appdata.Instance.sceneToLoad = Appdata.Instance.SceneInSave3;
            Appdata.Instance.currentScene = Appdata.Instance.sceneToLoad;
            StartCoroutine(PlayGame());
        });
    }

    private IEnumerator PlayGame()
    {
        StartCoroutine(TransitionController.Instance.EndTransition());
        yield return new WaitForSeconds(2.5f);
        SoundManager.Instance.PlayMusic(null,1f);
        SceneManager.LoadScene("LoadScene");
    }
}
