using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageController : MonoBehaviour
{
    public Text scoreText;
    public Text minuteText;
    public Text secondText;

    public float timeLimit = 500;
    public float clearScore;
    public GameObject arrowButton;
    public List<CoupleController> coupleControllers;
    public GameObject dialogCanvas;

    private float seconds = 500;
    private float timeLeft;
    private float currentScore;
    private bool gameEnded = false;
    // Start is called before the first frame update
    void Start()
    {
        timeLeft = timeLimit;
        currentScore = 0;
        SetCountText();
        dialogCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameEnded) {
            seconds -= Time.deltaTime;
            SetTimeText();
            checkGameEnd();
        }
        
    }

    void SetCountText()
    {
        //scoreText.text = DataRecord.score.ToString();
        scoreText.text = currentScore.ToString().PadLeft(5, '0');
    }

    void SetTimeText()
    {
        int minute = (int)seconds / 60;
        int second = (int)seconds % 60;
        minuteText.text = minute.ToString();
        secondText.text = second.ToString();
    }

    void checkGameEnd() {
        gameEnded = true;
        foreach (CoupleController coupleController in coupleControllers) {
            if (!coupleController.isInLove()) {
                gameEnded = false;
            }
        }
        if (gameEnded) {
            gameEnd(); 
        }
    }

    void gameEnd() {
        dialogCanvas.SetActive(true);
        arrowButton.SetActive(false);
    }

    public void loadScene(String sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
