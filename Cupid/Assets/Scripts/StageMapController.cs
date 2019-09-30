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

        stageNo1 = GameObject.Find("stageNo1");
        stageNo2 = GameObject.Find("stageNo2");
        stageNo3 = GameObject.Find("stageNo3");
        stageNo4 = GameObject.Find("stageNo4");
        stageNo5 = GameObject.Find("stageNo5");
        stageNo6 = GameObject.Find("stageNo6");
        stageNo7 = GameObject.Find("stageNo7");
        stageNo8 = GameObject.Find("stageNo8");
        stageNo9 = GameObject.Find("stageNo9");
        stageNo10 = GameObject.Find("stageNo10");

        star1_1 = GameObject.Find("star1_1");
        star1_2 = GameObject.Find("star1_2");
        star1_3 = GameObject.Find("star1_3");
        star2_1 = GameObject.Find("star2_1");
        star2_2 = GameObject.Find("star2_2");
        star2_3 = GameObject.Find("star2_3");
        star3_1 = GameObject.Find("star3_1");
        star3_2 = GameObject.Find("star3_2");
        star3_3 = GameObject.Find("star3_3");
        star4_1 = GameObject.Find("star4_1");
        star4_2 = GameObject.Find("star4_2");
        star4_3 = GameObject.Find("star4_3");
        star5_1 = GameObject.Find("star5_1");
        star5_2 = GameObject.Find("star5_2");
        star5_3 = GameObject.Find("star5_3");
        star6_1 = GameObject.Find("star6_1");
        star6_2 = GameObject.Find("star6_2");
        star6_3 = GameObject.Find("star6_3");
        star7_1 = GameObject.Find("star7_1");
        star7_2 = GameObject.Find("star7_2");
        star7_3 = GameObject.Find("star7_3");
        star8_1 = GameObject.Find("star8_1");
        star8_2 = GameObject.Find("star8_2");
        star8_3 = GameObject.Find("star8_3");
        star9_1 = GameObject.Find("star9_1");
        star9_2 = GameObject.Find("star9_2");
        star9_3 = GameObject.Find("star9_3");
        star10_1 = GameObject.Find("star10_1");
        star10_2 = GameObject.Find("star10_2");
        star10_3 = GameObject.Find("star10_3");

        stageBtn2.GetComponent<Button>().enabled = false;
        stageBtn3.GetComponent<Button>().enabled = false;
        stageBtn4.GetComponent<Button>().enabled = false;
        stageBtn5.GetComponent<Button>().enabled = false; 
        stageBtn6.GetComponent<Button>().enabled = false;
        stageBtn7.GetComponent<Button>().enabled = false;
        stageBtn8.GetComponent<Button>().enabled = false;
        stageBtn9.GetComponent<Button>().enabled = false;
        stageBtn10.GetComponent<Button>().enabled = false;

        //stageBtn1.GetComponent<Image>().alphaHitTestMinimumThreshold = alphaThreshold;

        stageNo2.SetActive(false);
        stageNo3.SetActive(false);
        stageNo4.SetActive(false);
        stageNo5.SetActive(false);
        stageNo6.SetActive(false);
        stageNo7.SetActive(false);
        stageNo8.SetActive(false);
        stageNo9.SetActive(false);
        stageNo10.SetActive(false);
        
        star1_1.SetActive(false);
        star1_2.SetActive(false);
        star1_3.SetActive(false);
        star2_1.SetActive(false);
        star2_2.SetActive(false);
        star2_3.SetActive(false);
        star3_1.SetActive(false);
        star3_2.SetActive(false);
        star3_3.SetActive(false);
        star4_1.SetActive(false);
        star4_2.SetActive(false);
        star4_3.SetActive(false);
        star5_1.SetActive(false);
        star5_2.SetActive(false);
        star5_3.SetActive(false);
        star6_1.SetActive(false);
        star6_2.SetActive(false);
        star6_3.SetActive(false);
        star7_1.SetActive(false);
        star7_2.SetActive(false);
        star7_3.SetActive(false);
        star8_1.SetActive(false);
        star8_2.SetActive(false);
        star8_3.SetActive(false);
        star9_1.SetActive(false);
        star9_2.SetActive(false);
        star9_3.SetActive(false);
        star10_1.SetActive(false);
        star10_2.SetActive(false);
        star10_3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void stageButton()
    {
        Debug.Log("click play button ...");
        GameObject.Find("StageMapWrapper").SetActive(false);
        SceneManager.GetSceneByName("StageScene").GetRootGameObjects()[0].SetActive(true);
    }
}
