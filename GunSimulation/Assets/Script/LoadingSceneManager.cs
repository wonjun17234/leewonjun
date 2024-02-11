using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadingSceneManager : MonoBehaviour
{
    public string nextScene;

    [SerializeField] Slider slider;

    public GameStartPage gameStartPage;


    public void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        gameStartPage.mainMenu.SetActive(false);
        slider.gameObject.SetActive(true);
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {

                slider.value = Mathf.Lerp(slider.value, op.progress, timer);
                if (slider.value >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                slider.value = Mathf.Lerp(slider.value, 1f, timer);
                if (slider.value == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
