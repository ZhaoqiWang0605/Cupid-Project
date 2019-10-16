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
    public GameObject dialogCanvas;
    public GameObject successDialog;
    public GameObject failDialog;


    private float seconds;
    private float currentScore = 0;
    private bool gameEnded = false;

    // Store number of star accquired by the player in current level
    private int starsNum;

    // Scores
    private float score1 = 500;
    private float score2 = 1000;
    private float score3 = 2000;

    public Audio audio;


    // Start is called before the first frame update
    void Start()
    {
        seconds = timeLimit;
        SetCountText();
        dialogCanvas.SetActive(false);
        successDialog.SetActive(false);
        failDialog.SetActive(false);
        Debug.Log(PlayerPrefs.GetString("currentStage") + " start");
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
        //scoreText.text = currentScore.ToString().PadLeft(5, '0');
        scoreText.text = currentScore.ToString();
    }

    void SetTimeText()
    {
        String minute = ((int)seconds / 60).ToString().PadLeft(2, '0');
        String second = ((int)seconds % 60).ToString().PadLeft(2, '0');
        timeText.text = minute + ":" + second;
        Debug.Log(PlayerPrefs.GetString("currentStage") + "time set");
    }

    void CheckGameEnded()
    {
        // First, check if all couples are in love, if they are, the player wins.
        gameEnded = true;
        foreach (CoupleController coupleController in coupleControllers)
        {
            if (!coupleController.isInLove())
            {
                gameEnded = false;
            }
        }
        if (gameEnded) {
            GameEnd();
            return;
        }

        // If there exist couples that are not in love yet, check if there's still time left,
        // if there isn't any tie left, the player loses.
        if (seconds <= 0)
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
        } else if (currentScore < score2)
        {
            starsNum = 1;
            GameEndSuccess(1);
        } else if (currentScore < score3)
        {
            starsNum = 2;
            GameEndSuccess(2);
        }
        else
        {
            starsNum = 3;
            GameEndSuccess(3);
        }
		audio.StopMusic();
    }

    public void UpdateScore(int type)
    {
        // 0 for people, 1 for item
        if (type == 0)
        {
            currentScore += 1000;
            scoreText.text = currentScore.ToString();
        }
    }

    void GameEndSuccess(int stars)
    {
        SaveData();
        print(stars);
        dialogCanvas.SetActive(true);
        successDialog.SetActive(true);
        failDialog.SetActive(false);
        arrowButton.SetActive(false);

        successDialog.GetComponent<SuccessDialog>().ShowStar(stars);
    }

    void GameEndFail()
    {
        SaveData();
        dialogCanvas.SetActive(true);
        successDialog.SetActive(false);
        failDialog.SetActive(true);
        arrowButton.SetActive(false);
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
        SceneManager.LoadScene("StageScene");
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

}
