using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageMapController : MonoBehaviour
{
    private GameObject stageBtn1, stageBtn2, stageBtn3, stageBtn4, stageBtn5, stageBtn6, stageBtn7, stageBtn8, stageBtn9, stageBtn10;
    void Start()
    {
        Debug.Log("stage map ...");

        stageBtn1 = GameObject.Find("playBtn1");
        stageBtn2 = GameObject.Find("playBtn2");
        stageBtn3 = GameObject.Find("playBtn3");
        stageBtn4 = GameObject.Find("playBtn4");
        stageBtn5 = GameObject.Find("playBtn5");
        stageBtn6 = GameObject.Find("playBtn6");
        stageBtn7 = GameObject.Find("playBtn7");
        stageBtn8 = GameObject.Find("playBtn8");
        stageBtn9 = GameObject.Find("playBtn9");
        stageBtn10 = GameObject.Find("playBtn10");
        //PlayerPrefs.DeleteAll();
    }

    public void stageButton()
    {
        Debug.Log("click play button ...");
        GameObject.Find("StageMapWrapper").SetActive(false);
        SceneManager.GetSceneByName("StageScene").GetRootGameObjects()[0].SetActive(true);
    }
}
