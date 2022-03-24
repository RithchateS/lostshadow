using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// เพิ่ม Scene ที่จะใช้ในหน้านี้
public enum SceneCollection
{
    Persistant, // 0
    MainMenu , // 1
    LoadScene , //2
    Tutorial01,// 3
    Tutorial02,//4
    Scenes01,//5
    Scenes02,//6
    Scenes03,//7
    Scenes04//8

}
public class LoadSceneManager : Singleton<LoadSceneManager>
{
    private GameObject _loadingCanvas ;
    public SceneCollection currentScene ;

    public void StartLoadingScene(SceneCollection scene){
        StartCoroutine(LoadScene(scene));
    }

    public void SetLoadingCanvas(GameObject lc){
        _loadingCanvas = lc ;
    }

    private IEnumerator LoadScene(SceneCollection scene){

        GameObject LoadScreenObject = Instantiate(_loadingCanvas);
        TMPro.TMP_Text LoadingPercentText = LoadScreenObject.transform.GetChild(1).GetComponent<TMPro.TMP_Text>() ;
        var currentProgress = 0f;
        yield return null;
        AsyncOperation async = SceneManager.UnloadSceneAsync(Enum.GetName(typeof(SceneCollection), currentScene));
        yield return new WaitForSeconds(1);
        async = SceneManager.LoadSceneAsync(Enum.GetName(typeof(SceneCollection), scene), LoadSceneMode.Single);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            currentProgress = Mathf.Clamp01(async.progress / 0.3f);
            LoadingPercentText.text = Mathf.Clamp(Mathf.RoundToInt(currentProgress / 5 * 100),0,100) + "%";
            if (currentProgress == 1f)
            {
                async.allowSceneActivation = true;
            }
            yield return null;
        }
        currentProgress = currentProgress / 5 * 100;
        while (currentProgress < 100)
        {
            if (currentProgress + Time.deltaTime * 100 <= 100)
            {
                currentProgress += Time.deltaTime * 150;
            }
            else
            {
                currentProgress = 100.00f;
            }
            LoadingPercentText.text = Mathf.Clamp(Mathf.RoundToInt(currentProgress), 0, 100) + "%";
            yield return null;
        }
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(Enum.GetName(typeof(SceneCollection), scene)));
        currentScene = scene ;
        LoadingPercentText.text = "0%";
        Destroy(LoadScreenObject);
    }
    


}
