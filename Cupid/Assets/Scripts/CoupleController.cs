using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoupleController : MonoBehaviour
{
    public ParticleSystem heartEffect;

    private bool inLove = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setInLove() {
        inLove = true;
        heartEffect.Play();
        Debug.Log("CoupleController.inLove(): Couple inLoved");
    }

    public bool isInLove() {
        return inLove;
    }
}
