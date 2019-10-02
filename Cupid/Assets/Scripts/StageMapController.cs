using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageMapController : MonoBehaviour
{
    //private MainSc mainScene;
    //private int numStages;
    //private Dictionary<int, GameObject> stageBtns;
    //private Dictionary<int, GameObject> stageNos;
    private GameObject stageBtn1, stageBtn2, stageBtn3, stageBtn4, stageBtn5, stageBtn6, stageBtn7, stageBtn8, stageBtn9, stageBtn10;
    private GameObject stageNo1, stageNo2, stageNo3, stageNo4, stageNo5, stageNo6, stageNo7, stageNo8, stageNo9, stageNo10;
    private GameObject star1_1, star1_2, star1_3,
                       star2_1, star2_2, star2_3, 
                       star3_1, star3_2, star3_3, 
                       star4_1, star4_2, star4_3, 
                       star5_1, star5_2, star5_3,
                       star6_1, star6_2, star6_3,
                       star7_1, star7_2, star7_3,
                       star8_1, star8_2, star8_3,
                       star9_1, star9_2, star9_3,
                       star10_1, star10_2, star10_3;
    //private float alphaThreshold = 0.9f;

    // Start is called before the first frame update
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
    }

    public void stageButton()
    {
        Debug.Log("click play button ...");
        GameObject.Find("StageMapWrapper").SetActive(false);
        SceneManager.GetSceneByName("StageScene").GetRootGameObjects()[0].SetActive(true);
    }
}
