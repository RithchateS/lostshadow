using System.Collections;
using Controller;
using Manager;
using Old.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Old.UI_Controller
{
    public class PlayMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject saveSlot1;
        [SerializeField] private GameObject saveSlot2;
        [SerializeField] private GameObject saveSlot3;
        [SerializeField] private GameObject deleteSave1;
        [SerializeField] private GameObject deleteSave2;
        [SerializeField] private GameObject deleteSave3;
        [SerializeField] private TMP_Text save1Info;
        [SerializeField] private TMP_Text save2Info;
        [SerializeField] private TMP_Text save3Info;
        private void Start()
        {
            LoadSaveInfo();
            AssignButtonFunctions();
        }



        /// <summary>
        /// Assign Functions to buttons in the Play Menu UI
        /// </summary>
        private void AssignButtonFunctions()
        {
            Appdata.Instance.currentScene = SceneCollection.MainMenu;
            deleteSave1.GetComponent<Button>().onClick.AddListener(delegate
            {
                GameSaveManager.Instance.DeleteSave(1);
                Appdata.Instance.isUsed1 = false;
                LoadSaveInfo();
            });deleteSave2.GetComponent<Button>().onClick.AddListener(delegate
            {
                GameSaveManager.Instance.DeleteSave(2);
                Appdata.Instance.isUsed2 = false;
                LoadSaveInfo();
            });deleteSave3.GetComponent<Button>().onClick.AddListener(delegate
            {
                GameSaveManager.Instance.DeleteSave(3);
                Appdata.Instance.isUsed3 = false;
                LoadSaveInfo();
            });
            saveSlot1.GetComponent<Button>().onClick.AddListener(delegate
            {
                GameSaveManager.Instance.SelectSaveSlot(0);
                Appdata.Instance.sceneToLoad = Appdata.Instance.sceneInSave1;
                Appdata.Instance.currentScene = Appdata.Instance.sceneToLoad;
                Destroy(saveSlot1.GetComponent<Button>());
                StartCoroutine(PlayGame());
            });
            saveSlot2.GetComponent<Button>().onClick.AddListener(delegate
            {
                GameSaveManager.Instance.SelectSaveSlot(1);
                Appdata.Instance.sceneToLoad = Appdata.Instance.sceneInSave2;
                Appdata.Instance.currentScene = Appdata.Instance.sceneToLoad;
                Destroy(saveSlot2.GetComponent<Button>());
                StartCoroutine(PlayGame());
            });
            saveSlot3.GetComponent<Button>().onClick.AddListener(delegate
            {
                GameSaveManager.Instance.SelectSaveSlot(2);
                Appdata.Instance.sceneToLoad = Appdata.Instance.sceneInSave3;
                Appdata.Instance.currentScene = Appdata.Instance.sceneToLoad;
                Destroy(saveSlot3.GetComponent<Button>());
                StartCoroutine(PlayGame());
            });
        }

        ///<summary>
        ///Play the game with the selected save slot
        ///</summary>
        private IEnumerator PlayGame()
        {
            StartCoroutine(TransitionController.Instance.EndTransition());
            yield return new WaitForSeconds(2.5f);
            SoundManager.Instance.PlayMusic(null,1f);
            SceneManager.LoadScene("LoadScene");
        }
        
        ///<summary>
        ///Update the save info in the UI.
        ///</summary>
        public void LoadSaveInfo()
        {
            if (Appdata.Instance.isUsed1)
            {
                save1Info.text = $"Save1 : {Appdata.Instance.sceneInSave1.ToString()}";
            }
            else
            {
                save1Info.text = "Save Empty";
            }
            if (Appdata.Instance.isUsed2)
            {
                save2Info.text = $"Save2 : {Appdata.Instance.sceneInSave2.ToString()}";
            }
            else
            {
                save2Info.text = "Save Empty";
            }
            if (Appdata.Instance.isUsed3)   
            {
                save3Info.text = $"Save3 : {Appdata.Instance.sceneInSave3.ToString()}";
            }
            else
            {
                save3Info.text = "Save Empty";
            }  
        }
    }
}
