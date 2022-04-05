using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum Menulist
{
    Main,
    Play,
    Credit
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
    

    // GameObject
    [SerializeField] private GameObject logo;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject playPage;
    [SerializeField] private GameObject creditPage;

    private void Start()
    {
        back_Button.GetComponent<Button>().onClick.AddListener(delegate { ChangePage(0); });
    }

    public void ChangePage(Menulist page)
    {
        CurrentPage = page;
    }

    public void ChangePage(int pageIndex)
    {
        CurrentPage = (Menulist) pageIndex;
    }

    private void OnMenuListChange()
    {
        logo.SetActive(false);
        mainMenu.SetActive(false);
        creditPage.SetActive(false);
        playPage.SetActive(false);
        back_Button.SetActive(false);

        switch (CurrentPage)
        {
            case Menulist.Main:
                logo.SetActive(true);
                mainMenu.SetActive(true);
                back_Button.SetActive(false);
                break;
            case Menulist.Play:
                logo.SetActive(false);
                playPage.SetActive(true);
                back_Button.SetActive(true);
                break;
            case Menulist.Credit:
                logo.SetActive(false);
                creditPage.SetActive(true);
                back_Button.SetActive(true);
                break;
        }
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

            if (GUILayout.Button("Play"))
            {
                menu.ChangePage(Menulist.Play);
            }

            if (GUILayout.Button("Credit"))
            {
                menu.ChangePage(Menulist.Credit);
            }

        }
    }
#endif