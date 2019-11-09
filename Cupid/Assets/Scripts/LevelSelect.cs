using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    private bool canSelect = false;
    private int starNum = 0;
    private string levelName;

    public GameObject[] stars;
    public GameObject stageNo;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("stage map ...");
        levelName = "level" + stageNo.GetComponent<Text>().text;
        starNum = PlayerPrefs.GetInt(levelName, 0);

        //默认开启第一关
        if (transform.parent.GetChild(1).name == gameObject.name || starNum > 0)
        {
            canSelect = true;

        } else { //前一关至少为一颗星时，开启本关
            int preStageNo = int.Parse(stageNo.GetComponent<Text>().text) - 1;
            string preLevelName = "level" + preStageNo.ToString();
            if (PlayerPrefs.GetInt(preLevelName) > 0)
            {
                canSelect = true;
            }
        }

        if (canSelect)
        {
            
            transform.Find("stageNo").gameObject.SetActive(true);
            transform.Find("lock").gameObject.SetActive(false);
            for (int i = 0; i < starNum; i++)
            {
                stars[i].SetActive(true);
            }
        }
    }

    public void Selected()
    {
        if (canSelect)
        {
            PlayBtnMusic();
            Debug.Log(levelName);
            //保存当前关卡名字在currentStage中
            PlayerPrefs.SetString("currentStage", levelName);
            //SceneManager.LoadScene("StageScene");
        }
    }

    private void PlayBtnMusic()
    {
        int preStageNo = int.Parse(stageNo.GetComponent<Text>().text);
        string whichBtn = "playBtn" + preStageNo;
        GameObject.Find(whichBtn).GetComponent<Audio>().PlayMusic();
    }
}
