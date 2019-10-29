using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadStage : MonoBehaviour
{
    private void Awake()
    {
        //Debug.Log(PlayerPrefs.GetString("currentStage"));
        Instantiate(Resources.Load(PlayerPrefs.GetString("currentStage")));
    }
}
