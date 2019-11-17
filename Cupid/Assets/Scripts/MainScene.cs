using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    //public GameObject bodyEyes_1;
    //public GameObject bodyEyes_2;
    //public bool isBodyEyes1;
    //public bool isBodyEyes2;
    public float time_slot;
    private float nextEffectTime;

    // Start is called before the first frame update
    void Start()
    {
        //bodyEyes_1.SetActive(isBodyEyes1);
        //bodyEyes_2.SetActive(isBodyEyes2);
        nextEffectTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextEffectTime)
        {
            //isBodyEyes1 = !isBodyEyes1;
            //isBodyEyes2 = !isBodyEyes2;
            //bodyEyes_1.SetActive(isBodyEyes1);
            //bodyEyes_2.SetActive(isBodyEyes2);
            nextEffectTime += time_slot;
        }
    }

    public void PlayButton()
    {
        GameObject.Find("playButton").GetComponent<Audio>().PlayMusic();
        //SceneManager.LoadScene("StageMapScene");
    }
}
