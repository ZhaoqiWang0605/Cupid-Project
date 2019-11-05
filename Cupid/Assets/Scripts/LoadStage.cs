using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadStage : MonoBehaviour
{
    private void Awake()
    {
        //Debug.Log(PlayerPrefs.GetString("currentStage"));
        Reset();
        Instantiate(Resources.Load(PlayerPrefs.GetString("currentStage")));
    }

    private void Reset()
    {
        Time.timeScale = 1;
    }
}
