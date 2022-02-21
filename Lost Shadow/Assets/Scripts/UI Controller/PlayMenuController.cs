using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayMenuController : MonoBehaviour
{
    [SerializeField]private GameObject saveSlot1;
    [SerializeField]private GameObject saveSlot2;
    [SerializeField]private GameObject saveSlot3;

    [SerializeField] private TMP_Text chaperterName1;
    [SerializeField] private TMP_Text chaperterName2;
    [SerializeField] private TMP_Text chaperterName3;
    private void Start()
    {
        saveSlot1.GetComponent<Button>().onClick.AddListener(delegate
        {
            GameSaveManager.Instance.SelectSaveSlot(0);
            Appdata.Instance.SceneToLoad = Appdata.Instance.SceneInSave1;
            Appdata.Instance.CurrentScene = Appdata.Instance.SceneToLoad;
            PlayGame();
        });
        saveSlot2.GetComponent<Button>().onClick.AddListener(delegate
        {
            GameSaveManager.Instance.SelectSaveSlot(1);
            Appdata.Instance.SceneToLoad = Appdata.Instance.SceneInSave2;
            Appdata.Instance.CurrentScene = Appdata.Instance.SceneToLoad;
            PlayGame();
        });
        saveSlot3.GetComponent<Button>().onClick.AddListener(delegate
        {
            GameSaveManager.Instance.SelectSaveSlot(2);
            Appdata.Instance.SceneToLoad = Appdata.Instance.SceneInSave3;
            Appdata.Instance.CurrentScene = Appdata.Instance.SceneToLoad;
            PlayGame();
        });
    }

    private void PlayGame()
    {
        SceneManager.LoadScene("LoadScene");
    }
}
