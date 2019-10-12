using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoupleController : MonoBehaviour
{
    public ParticleSystem heartEffect;
    public GameObject gameObject;
    public StageController stage;

    private bool inLove = false;
    // Start is called before the first frame update
    void Start()
    {
        gameObject = GameObject.Find("StageController");
        stage = gameObject.GetComponent<StageController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setInLove() {
        inLove = true;
        heartEffect.Play();
        Debug.Log("CoupleController.inLove(): Couple inLoved");
        stage.UpdateScore(0);
    }

    public bool isInLove() {
        return inLove;
    }
}
