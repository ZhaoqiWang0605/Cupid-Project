using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;

public class StageController : MonoBehaviour
{
    public Text scoreText;
    public Text timeText;

    public float timeLimit;
    public float clearScore;
    public List<CoupleController> coupleControllers;
    public GameObject arrowButton;
    public GameObject arrowSwitchButton;
    public GameObject dialogCanvas;
    public GameObject successDialog;
    public GameObject failDialog;
    public GameObject pauseDialog;

    private float seconds;
    private float currentScore = 0;
    private bool gameEnded = false;

    // Store number of star accquired by the player in current level
    private int starsNum;

    // Scores
    public int score1 = 500;
    public int score2 = 1000;
    public int score3 = 2000;



    // Start is called before the first frame update
    void Start()
    {
        seconds = timeLimit;
        SetCountText();
        dialogCanvas.SetActive(false);
        successDialog.SetActive(false);
        failDialog.SetActive(false);
        //Debug.Log(PlayerPrefs.GetString("currentStage") + " start");
    }

    // Update is called once per frame
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
        //Debug.Log(PlayerPrefs.GetString("currentStage") + "time set");
    }

    void CheckGameEnded()
    {
        bool allMatched = coupleControllers.TrueForAll((CoupleController obj) => obj.isInLove());
        //Debug.Log(allMatched);
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
        arrowButton.SetActive(false);
        arrowSwitchButton.SetActive(false);

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
        arrowButton.SetActive(false);
        arrowSwitchButton.SetActive(false);       
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
        arrowButton.SetActive(false);
        arrowSwitchButton.SetActive(false);
    }
    public void GameResume()
    {
        Time.timeScale = 1;

        dialogCanvas.SetActive(false);
        successDialog.SetActive(false);
        failDialog.SetActive(false);
        pauseDialog.SetActive(false);
        arrowButton.SetActive(true);
        arrowSwitchButton.SetActive(true);
    }

    private void stopBackgroundMusic()
    {
        this.GetComponent<AudioSource>().Stop();
    }
}
