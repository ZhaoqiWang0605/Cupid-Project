using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataRecord : MonoBehaviour
{
    // Start is called before the first frame update
    public static int score = 100;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ScoreUpdate(int increaseScore) 
    {
        score += increaseScore;
    }
}
