using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
	private bool canSelect = false;
    private int starNum = 0;
    private string levelName;

    // Start is called before the first frame update
    void Start()
    {
        levelName = PlayerPrefs.GetString("level" +
            GameObject.Find("stageNo").GetComponent<Text>().text);
        starNum = PlayerPrefs.GetInt(levelName, 0);
        if (transform.parent.GetChild(1).name == gameObject.name || starNum > 0)
		{
			canSelect = true;
		}
        if (canSelect)
		{
			transform.Find("stageNo").gameObject.SetActive(true);
			transform.Find("lock").gameObject.SetActive(false);
            if(starNum >= 1)
            {
                transform.Find("star1").gameObject.SetActive(true);
            }
            if(starNum >= 2)
            {
                transform.Find("star2").gameObject.SetActive(true);
            }
            if(starNum >= 3)
            {
                transform.Find("star3").gameObject.SetActive(true);
            }
		}
    }

    public void Selected()
	{
        if (canSelect)
		{
			PlayerPrefs.SetString("currentStage", gameObject.name);
			SceneManager.LoadScene("StageScene");
		}
	}
}
