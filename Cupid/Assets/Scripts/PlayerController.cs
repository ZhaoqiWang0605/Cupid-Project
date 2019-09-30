using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Text scoreText;
    public Text minuteText;
    public Text secondText;

    private double seconds = 500;
    // Start is called before the first frame update
    void Start()
    {
        SetCountText();
    }

    // Update is called once per frame
    void Update()
    {
        seconds -= Time.deltaTime;
        SetTimeText();
    }

    void SetCountText()
    {
        scoreText.text = DataRecord.score.ToString();
    }

    void SetTimeText()
    {
        int minute = (int)seconds / 60;
        int second = (int)seconds % 60;
        minuteText.text = minute.ToString();
        secondText.text = second.ToString();
    }
}
