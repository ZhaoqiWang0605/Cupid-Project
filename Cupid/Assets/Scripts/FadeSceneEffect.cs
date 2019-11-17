using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeSceneEffect : MonoBehaviour
{
    public float fadeSpeed;
    public GameObject mask;

    private RawImage rawImage;
    private string nextSceneName;
    private bool sceneStarting;
    private bool isClickNextBtn;

    // Start is called before the first frame update
    void Start()
    {
        mask.SetActive(true);
        rawImage = mask.GetComponent<RawImage>();
        nextSceneName = "";
        sceneStarting = true;
        isClickNextBtn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneStarting)
        {
            StartScene();
        }

        if (isClickNextBtn)
        {
            EndScene();
        }
    }

    private void FadeToClear()
    {
        Debug.Log("fadeSpeed: " + fadeSpeed);
        rawImage.color = Color.Lerp(rawImage.color, Color.clear, fadeSpeed * Time.deltaTime);
    }

    private void FadeToBlack()
    {
        Debug.Log("fadeSpeed: " + fadeSpeed);
        rawImage.color = Color.Lerp(rawImage.color, Color.black, fadeSpeed * Time.deltaTime);
    }

    void StartScene()
    {
        FadeToClear();
        if (rawImage.color.a < 0.01f)
        {
            rawImage.color = Color.clear;
            mask.SetActive(false);
            sceneStarting = false;
        }
    }

    void EndScene()
    {
        FadeToBlack();
        if (rawImage.color.a > 0.99f)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    public void ClickNextBtn(string sceneName)
    {
        nextSceneName = sceneName;
        mask.SetActive(true);
        isClickNextBtn = true;
        sceneStarting = false;
    }

    public void ResumeTime()
    {
        Time.timeScale = 1;
    }
}
