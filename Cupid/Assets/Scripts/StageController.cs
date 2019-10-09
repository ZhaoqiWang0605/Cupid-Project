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
    public Text minuteText;
    public Text secondText;

    public float timeLimit;
    public float clearScore;
    public List<CoupleController> coupleControllers;
    public GameObject arrowButton;
    public GameObject dialogCanvas;
    public GameObject successDialog;
    public GameObject failDialog;
    public CinemachineVirtualCamera vcam;
    public Transform vcamFollow;

    private float seconds;
    private float currentScore = 0;
    private bool gameEnded = false;
    // Start is called before the first frame update
    void Start()
    {
        seconds = timeLimit;
        SetCountText();
        dialogCanvas.SetActive(false);
        successDialog.SetActive(false);
        failDialog.SetActive(false);
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
        scoreText.text = currentScore.ToString().PadLeft(5, '0');
    }

    void SetTimeText()
    {
        int minute = (int)seconds / 60;
        int second = (int)seconds % 60;
        minuteText.text = minute.ToString();
        secondText.text = second.ToString();
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
            GameEndSuccess();
            return;
        }

        // If there exist couples that are not in love yet, check if there's still time left,
        // if there isn't any tie left, the player loses.
        if (seconds <= 0)
        {
            gameEnded = true;
            GameEndFail();
        }

    }

    void GameEndSuccess()
    {
        dialogCanvas.SetActive(true);
        successDialog.SetActive(true);
        failDialog.SetActive(false);
        arrowButton.SetActive(false);
    }

    void GameEndFail()
    {
        dialogCanvas.SetActive(true);
        successDialog.SetActive(false);
        failDialog.SetActive(true);
        arrowButton.SetActive(false);
    }

    public void loadScene(String sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void moveCamera(float deltaX)
    {
        Debug.Log("StageController.moveCamera(): " + deltaX);
        vcamFollow.position = new Vector2(vcamFollow.position.x + deltaX, vcamFollow.position.y);
    }
}
