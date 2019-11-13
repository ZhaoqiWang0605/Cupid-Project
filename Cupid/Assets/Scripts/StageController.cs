using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageController : MonoBehaviour
{
    public float timeLimit;
    public float clearScore;

    public List<CoupleController> coupleControllers = new List<CoupleController>();
    private GameObject arrowButtonPanel;
    private GameObject arrowSwitchPanel;

    private Text stageText;
    private Text scoreText;
    private Text timeText;

    private GameObject dialogCanvas;
    private GameObject successDialog;
    private GameObject failDialog;
    private GameObject pauseDialog;

    private float seconds;
    private float currentScore = 0;
    private bool gameEnded = false;

    // Store number of star accquired by the player in current level
    private int starsNum;

    // Scores
    public int score1 = 500;
    public int score2 = 1000;
    public int score3 = 2000;

    void Start()
    {
        // Get references
        GameObject couples = GameObject.Find("Couples");
        coupleControllers = new List<CoupleController>(FindObjectsOfType<CoupleController>());
        arrowButtonPanel = GameObject.Find("ArrowButtonPanel");
        arrowSwitchPanel = GameObject.Find("ArrowSwitchPanel");

        stageText = GameObject.Find("StageText").GetComponent<Text>(); 
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        timeText = GameObject.Find("TimeText").GetComponent<Text>();

        dialogCanvas = GameObject.Find("DialogCanvas");
        successDialog = GameObject.Find("SuccessDialog");
        failDialog = GameObject.Find("FailDialog");
        pauseDialog = GameObject.Find("PauseDialog");

        seconds = timeLimit;
        dialogCanvas.SetActive(false);
    }

    void Update()
    {
        if (!gameEnded)
        {
            seconds -= Time.deltaTime;
            SetTimeText();
            CheckGameEnded();
        }
    }

    void SetCountText()
    {
        scoreText.text = currentScore.ToString();
    }

    void SetTimeText()
    {
        String minute = ((int)seconds / 60).ToString().PadLeft(2, '0');
        String second = ((int)seconds % 60).ToString().PadLeft(2, '0');
        timeText.text = minute + ":" + second;
    }

    void CheckGameEnded()
    {
        bool allMatched = coupleControllers.TrueForAll((CoupleController obj) => obj.isInLove());
        if (allMatched || seconds <= 0)
        {
            gameEnded = true;
            GameEnd();
        }
    }

    void GameEnd()
    {
        if (currentScore < score1)
        {
            starsNum = 0;
            GameEndFail();
            return;
        }
        else if (currentScore < score2)
        {
            starsNum = 1;
        }
        else if (currentScore < score3)
        {
            starsNum = 2;
        }
        else
        {
            starsNum = 3;
        }
        StartCoroutine(Sleep());
    }

    IEnumerator Sleep()
    {
        yield return new WaitForSeconds(2);
        GameEndSuccess(starsNum);
    }

    public void UpdateScore(int s)
    {
        currentScore += s;
        scoreText.text = currentScore.ToString();
    }

    void GameEndSuccess(int stars)
    {
        SaveData();
        print(stars);
        //audio.StopMusic();
        stopBackgroundMusic();
        dialogCanvas.SetActive(true);
        successDialog.SetActive(true);
        failDialog.SetActive(false);
        pauseDialog.SetActive(false);
        arrowButtonPanel.SetActive(false);
        arrowSwitchPanel.SetActive(false);

        successDialog.GetComponent<SuccessDialog>().ShowStar(stars);
    }

    void GameEndFail()
    {
        SaveData();
        stopBackgroundMusic();
        dialogCanvas.SetActive(true);
        successDialog.SetActive(false);
        failDialog.SetActive(true);
        pauseDialog.SetActive(false);
        arrowButtonPanel.SetActive(false);
        arrowSwitchPanel.SetActive(false);       
    }

    public void loadScene(String sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void NextStage()
    {
        string currentStageName = PlayerPrefs.GetString("currentStage");
        int nextStageNum = int.Parse(currentStageName.Replace("level", "")) + 1;
        string nextStageName = "level" + nextStageNum;
        PlayerPrefs.SetString("currentStage", nextStageName);
        //SceneManager.LoadScene("StageScene");
    }

    //存储每一关星星的数量
    public void SaveData()
    {
        if (starsNum > PlayerPrefs.GetInt(PlayerPrefs.GetString("currentStage")))
        {
            PlayerPrefs.SetInt(PlayerPrefs.GetString("currentStage"), starsNum);
            Debug.Log("set starsNum to " + starsNum);
        }
    }

    public void IncreaseTime(int s)
    {
        seconds += s;
    }


    public void GamePause()
    {
        Time.timeScale = 0;

        dialogCanvas.SetActive(true);
        successDialog.SetActive(false);
        failDialog.SetActive(false);
        pauseDialog.SetActive(true);
        arrowButtonPanel.SetActive(false);
        arrowSwitchPanel.SetActive(false);
    }
    public void GameResume()
    {
        Time.timeScale = 1;

        dialogCanvas.SetActive(false);
        successDialog.SetActive(false);
        failDialog.SetActive(false);
        pauseDialog.SetActive(false);
        arrowButtonPanel.SetActive(true);
        arrowSwitchPanel.SetActive(true);
    }

    private void stopBackgroundMusic()
    {
        this.GetComponent<AudioSource>().Stop();
    }
}
